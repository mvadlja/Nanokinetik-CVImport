
-- GetEntities
CREATE PROCEDURE [dbo].[proc_PRODUCT_PACKAGING_MATERIAL_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_packaging_material_mn_PK], [product_FK], [type_FK]
	FROM [dbo].[PRODUCT_PACKAGING_MATERIAL_MN]
END