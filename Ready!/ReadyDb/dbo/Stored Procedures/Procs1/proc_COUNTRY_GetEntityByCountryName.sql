-- GetEntityByCountryName
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetEntityByCountryName]
	@name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[COUNTRY].[country_PK], [dbo].[COUNTRY].[name], [dbo].[COUNTRY].[abbreviation], [dbo].[COUNTRY].[region], [dbo].[COUNTRY].[code]
	FROM [dbo].[COUNTRY]
	WHERE ([dbo].[COUNTRY].[name] LIKE '%' + @name + '%' OR @name IS NULL)

END
