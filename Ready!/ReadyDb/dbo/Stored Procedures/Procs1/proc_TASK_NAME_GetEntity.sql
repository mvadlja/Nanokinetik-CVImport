-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TASK_NAME_GetEntity]
	@task_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_name_PK], [task_name]
	FROM [dbo].[TASK_NAME]
	WHERE [task_name_PK] = @task_name_PK
END
