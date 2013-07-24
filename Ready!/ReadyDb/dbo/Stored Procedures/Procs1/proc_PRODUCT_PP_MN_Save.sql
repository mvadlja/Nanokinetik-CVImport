-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_Save]
	@product_mn_PK int = NULL,
	@product_FK int = NULL,
	@pp_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_PP_MN]
	SET
	[product_FK] = @product_FK,
	[pp_FK] = @pp_FK
	WHERE [product_mn_PK] = @product_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_PP_MN]
		([product_FK], [pp_FK])
		VALUES
		(@product_FK, @pp_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
