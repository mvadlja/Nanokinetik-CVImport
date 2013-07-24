-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TASK_GetTabMenuItemsCount]
	@task_PK int = NULL,
	@ResponsibleUserPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @AlertRecordsCount NVARCHAR(MAX) = NULL;

	DECLARE @PersonEmail NVARCHAR(500) = (SELECT email FROM PERSON WHERE person_PK = CONVERT(INT, @ResponsibleUserPk));
	
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
			(rmd.is_automatic != 1 or reminderStatus.name != 'Dismissed') AND (rmd.reminder_type LIKE 'Task' OR rmd.reminder_type LIKE 'Task Document') AND rmd.related_entity_FK = @task_PK;

	SELECT 
		(select count(*) from dbo.TASK_DOCUMENT_MN where dbo.TASK_DOCUMENT_MN.task_FK = @task_PK) as 'TaskDocList',
		(SELECT @AlertRecordsCount) as 'TaskAlertList',
		(select COUNT(*) from SUBMISSION_UNIT as su	where su.task_FK = @task_PK) as 'TaskSubUnitList'
END
