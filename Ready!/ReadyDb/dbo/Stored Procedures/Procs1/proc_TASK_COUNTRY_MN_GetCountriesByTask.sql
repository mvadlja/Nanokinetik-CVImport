-- GetUsersInRolesByUserID
CREATE PROCEDURE  [dbo].[proc_TASK_COUNTRY_MN_GetCountriesByTask]
	@task_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[TASK_COUNTRY_MN].*
	FROM [dbo].[TASK_COUNTRY_MN]
	WHERE [dbo].[TASK_COUNTRY_MN].[task_FK] = @task_PK

END
