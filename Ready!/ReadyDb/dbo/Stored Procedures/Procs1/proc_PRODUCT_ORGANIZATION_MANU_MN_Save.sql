-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ORGANIZATION_MANU_MN_Save]
	@product_organization_manu_mn_PK int = NULL,
	@organization_FK int = NULL,
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_ORGANIZATION_MANU_MN]
	SET
	[organization_FK] = @organization_FK,
	[product_FK] = @product_FK
	WHERE [product_organization_manu_mn_PK] = @product_organization_manu_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_ORGANIZATION_MANU_MN]
		([organization_FK], [product_FK])
		VALUES
		(@organization_FK, @product_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
