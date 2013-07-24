-- Delete
CREATE PROCEDURE  [dbo].[proc_TASK_NAME_Delete]
	@task_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TASK_NAME] WHERE [task_name_PK] = @task_name_PK
END
