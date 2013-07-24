-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_GetTabMenuItemsCount]
	@activity_PK int = NULL,
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
	where am.TableName = 'ACTIVITY' and ad.PKValue = CAST (@activity_PK AS NVARCHAR(MAX));	

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
			(rmd.is_automatic != 1 or reminderStatus.name != 'Dismissed') AND (rmd.reminder_type LIKE 'Activity' OR rmd.reminder_type LIKE 'A Document') AND rmd.related_entity_FK = @activity_PK;
			
    WITH cte_result AS (
	SELECT (select count(*) from dbo.ACTIVITY_DOCUMENT_MN
			where dbo.ACTIVITY_DOCUMENT_MN.activity_FK = @activity_PK) as 'ActDocList',
			(select count(*) from dbo.TIME_UNIT
			where dbo.TIME_UNIT.activity_FK = @activity_PK) as 'ActTimeUnitList',
			(select count(*) from dbo.TASK
			where dbo.TASK.activity_FK = @activity_PK) as 'ActTaskList',
			(SELECT COUNT(DISTINCT su.subbmission_unit_PK) FROM [dbo].SUBMISSION_UNIT as su
			LEFT JOIN [dbo].TASK t ON t.task_PK = su.task_FK
			join dbo.TASK_NAME on task_name_FK=task_name_PK
			JOIN ACTIVITY on activity_PK=activity_FK
			where t.activity_FK=@activity_PK) as 'ActSubUnitList',
			(select @AlertRecordsCount) as 'ActAlertList',
			(select @AuditRecordsCount) as 'ActAuditTrailList' 
     )

     SELECT 
         cte_result.*, 
		 cte_result.ActDocList AS ActMyDocList,
		 cte_result.ActTaskList AS ActMyTaskList,
		 cte_result.ActTimeUnitList AS ActMyTimeUnitList,
		 cte_result.ActSubUnitList AS ActMySubUnitList,
		 cte_result.ActAlertList AS ActMyAlertList,
		 cte_result.ActAuditTrailList AS ActMyAuditTrailList
     FROM cte_result

END
