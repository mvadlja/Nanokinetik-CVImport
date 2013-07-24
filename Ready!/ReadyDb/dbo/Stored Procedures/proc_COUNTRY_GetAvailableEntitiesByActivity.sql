-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAvailableEntitiesByActivity]
	@ActivityPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE [dbo].[COUNTRY].country_PK NOT IN
	(	
		SELECT country_FK 
		FROM dbo.[ACTIVITY_COUNTRY_MN] 
		WHERE activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL AND country_FK is not null
	)
	ORDER BY [custom_sort_ID] ASC
END