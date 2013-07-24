-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_Save]
	@product_submission_unit_PK int = NULL,
	@product_FK int = NULL,
	@submission_unit_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_SUB_UNIT_MN]
	SET
	[product_FK] = @product_FK,
	[submission_unit_FK] = @submission_unit_FK
	WHERE [product_submission_unit_PK] = @product_submission_unit_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_SUB_UNIT_MN]
		([product_FK], [submission_unit_FK])
		VALUES
		(@product_FK, @submission_unit_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
