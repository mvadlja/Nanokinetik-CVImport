-- GetOrganisationInRolesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_PP_ACTIVE_INGREDIENT_GetIngredientsByPPPK]
	@pharmaceutical_product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PP_ACTIVE_INGREDIENT].*
	FROM [dbo].[PP_ACTIVE_INGREDIENT]
	WHERE ([dbo].[PP_ACTIVE_INGREDIENT].[pp_FK] = @pharmaceutical_product_FK OR @pharmaceutical_product_FK IS NULL)

END
