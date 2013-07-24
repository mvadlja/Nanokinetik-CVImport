-- GetOrganisationInRolesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_PP_AR_MN_GetAdminRoutesByPPPK]
	@pharmaceutical_product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PP_AR_MN].[pp_ar_mn_PK], [dbo].[PP_AR_MN].[pharmaceutical_product_FK], [dbo].[PP_AR_MN].[admin_route_FK]
	FROM [dbo].[PP_AR_MN]
	WHERE ([dbo].[PP_AR_MN].[pharmaceutical_product_FK] = @pharmaceutical_product_FK OR @pharmaceutical_product_FK IS NULL)

END
