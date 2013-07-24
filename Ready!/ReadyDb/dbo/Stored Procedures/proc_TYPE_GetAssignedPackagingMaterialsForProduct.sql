-- dasdas
CREATE PROCEDURE  [dbo].[proc_TYPE_GetAssignedPackagingMaterialsForProduct]
	@ProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[TYPE].*
	FROM [dbo].[TYPE]
	WHERE [dbo].[TYPE].[group] = 'PM'
	and [dbo].[TYPE].[type_PK] IN
	(
		select type_FK from dbo.PRODUCT_PACKAGING_MATERIAL_MN
		where product_FK = @ProductPk
	)

END