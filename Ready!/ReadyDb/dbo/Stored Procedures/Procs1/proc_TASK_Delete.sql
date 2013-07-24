-- Delete
CREATE PROCEDURE  [dbo].[proc_TASK_Delete]
	@task_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TASK] WHERE [task_PK] = @task_PK
END
