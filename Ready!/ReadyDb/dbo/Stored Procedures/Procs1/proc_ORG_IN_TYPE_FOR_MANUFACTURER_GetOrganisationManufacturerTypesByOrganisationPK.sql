-- GetOrganisationPartnerTypesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetOrganisationManufacturerTypesByOrganisationPK]
	@organization_FK int = NULL,
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[org_in_type_for_manufacturer_ID], [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[organization_FK], [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[org_type_for_manu_FK], [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[product_FK], [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[substance_FK]
	FROM [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER]
	WHERE ([dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[organization_FK] = @organization_FK OR @organization_FK IS NULL) AND
	([dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[product_FK] = @product_FK OR @product_FK IS NULL)

END
