
-- Delete
CREATE PROCEDURE [dbo].[proc_PRODUCT_PACKAGING_MATERIAL_MN_Delete]
	@product_packaging_material_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_PACKAGING_MATERIAL_MN] WHERE [product_packaging_material_mn_PK] = @product_packaging_material_mn_PK
END