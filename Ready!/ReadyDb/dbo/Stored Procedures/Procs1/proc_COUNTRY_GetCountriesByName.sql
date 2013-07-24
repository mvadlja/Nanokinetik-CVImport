-- GetEntityByCountryName
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetCountriesByName]
	@name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[COUNTRY].[abbreviation] + ' - ' + [dbo].[COUNTRY].[name] as name, [dbo].[COUNTRY].[country_PK]
	FROM [dbo].[COUNTRY]
	WHERE ([dbo].[COUNTRY].[name] LIKE '%' + @name + '%' OR @name IS NULL)

END
