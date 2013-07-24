-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetTabMenuItemsCount]
	@document_PK int = NULL,
	@ResponsibleUserPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @AuditRecordsCount NVARCHAR(MAX) = NULL;
	DECLARE @AlertRecordsCount NVARCHAR(MAX) = NULL;

	DECLARE @PersonEmail NVARCHAR(500) = (SELECT email FROM PERSON WHERE person_PK = CONVERT(INT, @ResponsibleUserPk));

	select @AuditRecordsCount = count (distinct SessionToken)
	from AuditingMaster am
	left join [AuditingDetails] ad on am.IDAuditingMaster = ad.MasterID		
	where am.TableName = 'DOCUMENT' and ad.PKValue = CAST (@document_PK AS NVARCHAR(MAX));	

	SELECT @AlertRecordsCount = COUNT(DISTINCT  rmd.reminder_PK) 
			FROM [dbo].[REMINDER] rmd
			LEFT JOIN dbo.REMINDER_EMAIL_RECIPIENT rmdERcv ON rmdERcv.reminder_FK = rmd.reminder_PK
			LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = rmd.reminder_PK
			LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
			WHERE (rmd.responsible_user_FK = @ResponsibleUserPk OR
			(rmdERcv.person_FK = @ResponsibleUserPk AND @ResponsibleUserPk IS NOT NULL OR
			rmd.additional_emails LIKE @PersonEmail OR 
			rmd.additional_emails LIKE @PersonEmail + ',%' OR
			rmd.additional_emails LIKE '%, ' + @PersonEmail +',%' OR
			rmd.additional_emails LIKE '%, ' + @PersonEmail)) AND
			(rmd.is_automatic != 1 or reminderStatus.name != 'Dismissed') AND (rmd.reminder_type LIKE '%Document%') AND rmd.related_entity_FK = @document_PK;
			
	SELECT (select @AuditRecordsCount) as 'Doc',
	 (SELECT @AlertRecordsCount) as 'DocAlertList',
	 (SELECT @AuditRecordsCount) as 'DocAuditTrailList'
END
