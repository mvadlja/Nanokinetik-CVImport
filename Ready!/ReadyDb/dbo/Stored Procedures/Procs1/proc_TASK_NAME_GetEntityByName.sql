-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TASK_NAME_GetEntityByName]
	@task_name varchar(max) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_name_PK], [task_name]
	FROM [dbo].[TASK_NAME]
	WHERE [task_name] = @task_name  
END
