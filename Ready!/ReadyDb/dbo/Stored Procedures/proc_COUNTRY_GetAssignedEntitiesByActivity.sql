-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAssignedEntitiesByActivity]
	@ActivityPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	JOIN [dbo].[ACTIVITY_COUNTRY_MN] ON [dbo].[ACTIVITY_COUNTRY_MN].country_FK = [dbo].[COUNTRY].country_PK
	WHERE [dbo].[ACTIVITY_COUNTRY_MN].activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL
	ORDER BY [dbo].[COUNTRY].[custom_sort_ID] ASC
END