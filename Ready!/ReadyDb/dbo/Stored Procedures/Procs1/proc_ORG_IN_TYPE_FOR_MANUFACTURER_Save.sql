-- Save
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_MANUFACTURER_Save]
	@org_in_type_for_manufacturer_ID int = NULL,
	@organization_FK int = NULL,
	@org_type_for_manu_FK int = NULL,
	@product_FK int = NULL,
	@substance_FK int = NULL,
	@ManufacturerName NVARCHAR(100) = NULL,
	@ManufacturerTypeName NVARCHAR(100) = NULL,
	@SubstanceName NVARCHAR(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER]
	SET
	[organization_FK] = @organization_FK,
	[org_type_for_manu_FK] = @org_type_for_manu_FK,
	[product_FK] = @product_FK,
	[substance_FK] = @substance_FK
	WHERE [org_in_type_for_manufacturer_ID] = @org_in_type_for_manufacturer_ID

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER]
		([organization_FK], [org_type_for_manu_FK], [product_FK], [substance_FK])
		VALUES
		(@organization_FK, @org_type_for_manu_FK, @product_FK, @substance_FK )

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
