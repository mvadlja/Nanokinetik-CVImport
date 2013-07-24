-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ORGANIZATION_MANU_MN_Delete]
	@product_organization_manu_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_ORGANIZATION_MANU_MN] WHERE [product_organization_manu_mn_PK] = @product_organization_manu_mn_PK
END
