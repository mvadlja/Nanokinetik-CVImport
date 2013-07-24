-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PROJECT_COUNTRY_MN_GetEntity]
	@project_country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[project_country_PK], [project_FK], [country_FK]
	FROM [dbo].[PROJECT_COUNTRY_MN]
	WHERE [project_country_PK] = @project_country_PK
END
