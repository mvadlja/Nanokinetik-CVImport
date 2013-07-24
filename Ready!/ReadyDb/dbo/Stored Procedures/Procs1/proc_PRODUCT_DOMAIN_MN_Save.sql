-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOMAIN_MN_Save]
	@product_domain_mn_PK int = NULL,
	@product_FK int = NULL,
	@domain_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_DOMAIN_MN]
	SET
	[product_FK] = @product_FK,
	[domain_FK] = @domain_FK
	WHERE [product_domain_mn_PK] = @product_domain_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_DOMAIN_MN]
		([product_FK], [domain_FK])
		VALUES
		(@product_FK, @domain_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
