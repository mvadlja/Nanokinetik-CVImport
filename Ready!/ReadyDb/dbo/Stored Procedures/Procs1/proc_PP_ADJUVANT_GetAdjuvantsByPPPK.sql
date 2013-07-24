-- GetOrganisationInRolesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_PP_ADJUVANT_GetAdjuvantsByPPPK]
	@pharmaceutical_product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PP_ADJUVANT].*
	FROM [dbo].[PP_ADJUVANT]
	WHERE ([dbo].[PP_ADJUVANT].[pp_FK] = @pharmaceutical_product_FK OR @pharmaceutical_product_FK IS NULL)

END
