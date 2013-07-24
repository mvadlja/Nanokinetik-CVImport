-- GetEntitiesCombined
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetEntitiesCombined]
	
AS
BEGIN
	SET NOCOUNT ON;
	select [region]=COALESCE(abbreviation,'') + ' - ' + name FROM [dbo].[COUNTRY]
	SELECT
	[country_PK], [name], [abbreviation], [region], [code]
	FROM [dbo].[COUNTRY]
END
