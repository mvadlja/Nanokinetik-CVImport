-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PP_ACTIVE_INGREDIENT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activeingredient_PK], [substancecode_FK], [resolutionmode], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [highamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [userID], [strength_value], [strength_unit], [ExpressedBy_FK], [concise]
	FROM [dbo].[PP_ACTIVE_INGREDIENT]
END
