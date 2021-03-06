﻿-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAssignedEntitiesByProject]
	@ProjectPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	JOIN [dbo].[PROJECT_COUNTRY_MN] ON [dbo].[PROJECT_COUNTRY_MN].country_FK = [dbo].[COUNTRY].country_PK
	WHERE [dbo].[PROJECT_COUNTRY_MN].project_FK = @ProjectPk AND @ProjectPk IS NOT NULL
	ORDER BY [dbo].[COUNTRY].[custom_sort_ID] ASC
END