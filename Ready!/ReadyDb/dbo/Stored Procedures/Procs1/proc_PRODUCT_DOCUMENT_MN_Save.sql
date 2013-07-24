-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_Save]
	@product_document_mn_PK int = NULL,
	@product_FK int = NULL,
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_DOCUMENT_MN]
	SET
	[product_FK] = @product_FK,
	[document_FK] = @document_FK
	WHERE [product_document_mn_PK] = @product_document_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_DOCUMENT_MN]
		([product_FK], [document_FK])
		VALUES
		(@product_FK, @document_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
