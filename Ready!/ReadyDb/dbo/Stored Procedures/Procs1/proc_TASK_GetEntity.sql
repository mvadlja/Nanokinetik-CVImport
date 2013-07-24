-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TASK_GetEntity]
	@task_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_PK], [activity_FK], [user_FK], [task_name_FK], [description], [comment], [type_internal_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [POM_internal_status],  [automatic_alerts_on],[prevent_start_date_alert], [prevent_exp_finish_date_alert], [task_ID]
	FROM [dbo].[TASK]
	WHERE [task_PK] = @task_PK
END
