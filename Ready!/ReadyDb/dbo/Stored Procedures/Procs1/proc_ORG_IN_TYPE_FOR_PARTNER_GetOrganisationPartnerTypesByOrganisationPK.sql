-- GetOrganisationPartnerTypesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_PARTNER_GetOrganisationPartnerTypesByOrganisationPK]
	@organization_FK int = NULL,
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[ORG_IN_TYPE_FOR_PARTNER].[org_in_type_for_partner_ID], [dbo].[ORG_IN_TYPE_FOR_PARTNER].[organization_FK], [dbo].[ORG_IN_TYPE_FOR_PARTNER].[org_type_for_partner_FK], [dbo].[ORG_IN_TYPE_FOR_PARTNER].[product_FK]
	FROM [dbo].[ORG_IN_TYPE_FOR_PARTNER]
	WHERE ([dbo].[ORG_IN_TYPE_FOR_PARTNER].[organization_FK] = @organization_FK OR @organization_FK IS NULL)AND
	([dbo].[ORG_IN_TYPE_FOR_PARTNER].[product_FK] = @product_FK OR @product_FK IS NULL)

END
