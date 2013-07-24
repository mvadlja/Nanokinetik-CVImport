-- GetEntity
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetEntity]
	@country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE [country_PK] = @country_PK
END
