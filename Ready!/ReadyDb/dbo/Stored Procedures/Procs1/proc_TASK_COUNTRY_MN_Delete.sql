-- Delete
CREATE PROCEDURE  [dbo].[proc_TASK_COUNTRY_MN_Delete]
	@task_country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TASK_COUNTRY_MN] WHERE [task_country_PK] = @task_country_PK
END
