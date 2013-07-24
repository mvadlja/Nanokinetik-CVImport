-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_COUNTRY_MN_Save]
	@product_country_mn_PK int = NULL,
	@country_FK int = NULL,
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_COUNTRY_MN]
	SET
	[country_FK] = @country_FK,
	[product_FK] = @product_FK
	WHERE [product_country_mn_PK] = @product_country_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_COUNTRY_MN]
		([country_FK], [product_FK])
		VALUES
		(@country_FK, @product_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
