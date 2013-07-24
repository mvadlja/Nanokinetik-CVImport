-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
END
