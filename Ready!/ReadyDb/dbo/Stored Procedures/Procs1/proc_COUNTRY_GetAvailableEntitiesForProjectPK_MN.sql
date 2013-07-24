-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAvailableEntitiesForProjectPK_MN]
	@project_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE
	country_PK
	NOT IN
	(
		select country_FK from dbo.PROJECT_COUNTRY_MN where project_FK = @project_PK
	)
	ORDER BY [custom_sort_ID] ASC
END
