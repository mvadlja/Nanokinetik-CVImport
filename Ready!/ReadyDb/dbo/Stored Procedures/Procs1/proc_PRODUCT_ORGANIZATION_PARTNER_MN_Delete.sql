-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ORGANIZATION_PARTNER_MN_Delete]
	@product_organization_partner_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_ORGANIZATION_PARTNER_MN] WHERE [product_organization_partner_mn_PK] = @product_organization_partner_mn_PK
END
