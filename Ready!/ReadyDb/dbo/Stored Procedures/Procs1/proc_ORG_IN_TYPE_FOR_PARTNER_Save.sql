-- Save
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_PARTNER_Save]
	@org_in_type_for_partner_ID int = NULL,
	@organization_FK int = NULL,
	@org_type_for_partner_FK int = NULL,
	@product_FK int = null,
	@PartnerName NVARCHAR(100) = NULL,
	@PartnerTypeName NVARCHAR(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ORG_IN_TYPE_FOR_PARTNER]
	SET
	[organization_FK] = @organization_FK,
	[org_type_for_partner_FK] = @org_type_for_partner_FK,
	[product_FK] = @product_FK
	WHERE [org_in_type_for_partner_ID] = @org_in_type_for_partner_ID

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ORG_IN_TYPE_FOR_PARTNER]
		([organization_FK], [org_type_for_partner_FK],[product_FK])
		VALUES
		(@organization_FK, @org_type_for_partner_FK,@product_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
