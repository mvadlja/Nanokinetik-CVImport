-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetEntitiesCustomSort]
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE [country_PK] NOT IN (103) -- EMEA
	ORDER BY [custom_sort_ID] ASC
END
