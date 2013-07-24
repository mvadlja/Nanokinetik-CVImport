-- GetRolesByUserID
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetCountriesByTask]
	@task_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].COUNTRY.*
	FROM [dbo].COUNTRY
	LEFT JOIN [dbo].[TASK_COUNTRY_MN] ON COUNTRY.country_PK = [TASK_COUNTRY_MN].country_FK 
	WHERE [dbo].[TASK_COUNTRY_MN].task_FK = @task_PK

END
