-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_Save]
	@product_pi_mn_PK int = NULL,
	@product_indications_FK int = NULL,
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_PI_MN]
	SET
	[product_indications_FK] = @product_indications_FK,
	[product_FK] = @product_FK
	WHERE [product_pi_mn_PK] = @product_pi_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_PI_MN]
		([product_indications_FK], [product_FK])
		VALUES
		(@product_indications_FK, @product_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
