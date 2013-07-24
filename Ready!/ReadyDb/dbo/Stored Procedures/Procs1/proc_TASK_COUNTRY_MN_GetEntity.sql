-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TASK_COUNTRY_MN_GetEntity]
	@task_country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_country_PK], [task_FK], [country_FK]
	FROM [dbo].[TASK_COUNTRY_MN]
	WHERE [task_country_PK] = @task_country_PK
END
