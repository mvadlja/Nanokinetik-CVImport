
-- GetEntity
CREATE PROCEDURE [dbo].[proc_PRODUCT_PACKAGING_MATERIAL_MN_GetEntity]
	@product_packaging_material_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_packaging_material_mn_PK], [product_FK], [type_FK]
	FROM [dbo].[PRODUCT_PACKAGING_MATERIAL_MN]
	WHERE [product_packaging_material_mn_PK] = @product_packaging_material_mn_PK
END