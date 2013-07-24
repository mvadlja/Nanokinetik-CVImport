-- Delete
CREATE PROCEDURE  [dbo].[proc_TASK_COUNTRY_MN_DeleteByTaskPK]
	@task_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.TASK_COUNTRY_MN WHERE task_FK = @task_PK;
END
