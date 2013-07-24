-- GetEntities
create PROCEDURE  [dbo].[proc_PP_ADMINISTRATION_ROUTE_GetEntitiesByPharmaceuticalProduct]
	@PharmaceuticalProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[dbo].PP_ADMINISTRATION_ROUTE.[adminroute_PK], [dbo].PP_ADMINISTRATION_ROUTE.[adminroutecode], [dbo].PP_ADMINISTRATION_ROUTE.[resolutionmode], [dbo].PP_ADMINISTRATION_ROUTE.[ev_code]
	FROM [dbo].PP_AR_MN
	JOIN [dbo].PP_ADMINISTRATION_ROUTE ON [dbo].PP_ADMINISTRATION_ROUTE.adminroute_PK = [dbo].PP_AR_MN.admin_route_FK
	WHERE [dbo].PP_AR_MN.pharmaceutical_product_FK = @PharmaceuticalProductPk AND @PharmaceuticalProductPk IS NOT NULL
END