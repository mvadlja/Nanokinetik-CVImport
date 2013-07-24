
-- Save
CREATE PROCEDURE [dbo].[proc_PRODUCT_PACKAGING_MATERIAL_MN_Save]
	@product_packaging_material_mn_PK int = NULL,
	@product_FK int = NULL,
	@type_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_PACKAGING_MATERIAL_MN]
	SET
	[product_FK] = @product_FK,
	[type_FK] = @type_FK
	WHERE [product_packaging_material_mn_PK] = @product_packaging_material_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_PACKAGING_MATERIAL_MN]
		([product_FK], [type_FK])
		VALUES
		(@product_FK, @type_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END