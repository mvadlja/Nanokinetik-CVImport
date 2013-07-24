-- GetCountriesByActivity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_COUNTRY_MN_GetCountriesByActivity]
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.activity_country_PK, a.activity_PK, c.country_PK, c.abbreviation
	FROM [dbo].[ACTIVITY_COUNTRY_MN] mn
	LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = mn.activity_FK
	LEFT JOIN [dbo].[COUNTRY] c ON c.country_PK = mn.country_FK
	WHERE (mn.activity_FK = @activity_FK OR @activity_FK IS NULL)

END
