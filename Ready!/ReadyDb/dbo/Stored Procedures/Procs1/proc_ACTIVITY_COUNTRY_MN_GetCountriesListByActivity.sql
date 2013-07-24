-- GetCountriesByActivity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_COUNTRY_MN_GetCountriesListByActivity]
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT activity_country_PK, activity_FK, country_FK
	FROM [dbo].[ACTIVITY_COUNTRY_MN] mn
	WHERE activity_FK = @activity_FK

END
