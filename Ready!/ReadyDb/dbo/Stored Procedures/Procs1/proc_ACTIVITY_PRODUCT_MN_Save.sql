-- Save
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PRODUCT_MN_Save]
	@activity_product_PK int = NULL,
	@activity_FK int = NULL,
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY_PRODUCT_MN]
	SET
	[activity_FK] = @activity_FK,
	[product_FK] = @product_FK
	WHERE [activity_product_PK] = @activity_product_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY_PRODUCT_MN]
		([activity_FK], [product_FK])
		VALUES
		(@activity_FK, @product_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
