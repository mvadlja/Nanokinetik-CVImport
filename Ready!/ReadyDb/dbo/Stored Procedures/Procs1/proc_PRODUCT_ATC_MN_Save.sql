-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ATC_MN_Save]
	@product_atc_mn_PK int = NULL,
	@product_FK int = NULL,
	@atc_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_ATC_MN]
	SET
	[product_FK] = @product_FK,
	[atc_FK] = @atc_FK
	WHERE [product_atc_mn_PK] = @product_atc_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_ATC_MN]
		([product_FK], [atc_FK])
		VALUES
		(@product_FK, @atc_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
