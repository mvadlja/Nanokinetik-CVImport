-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PROJECT_COUNTRY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[project_country_PK], [project_FK], [country_FK]
	FROM [dbo].[PROJECT_COUNTRY_MN]
END
