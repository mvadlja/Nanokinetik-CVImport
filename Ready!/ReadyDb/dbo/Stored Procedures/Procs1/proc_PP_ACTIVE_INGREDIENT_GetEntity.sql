-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PP_ACTIVE_INGREDIENT_GetEntity]
	@activeingredient_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activeingredient_PK], [substancecode_FK], [resolutionmode], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [highamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [userID], [strength_value], [strength_unit], [ExpressedBy_FK], [concise]
	FROM [dbo].[PP_ACTIVE_INGREDIENT]
	WHERE [activeingredient_PK] = @activeingredient_PK
END
