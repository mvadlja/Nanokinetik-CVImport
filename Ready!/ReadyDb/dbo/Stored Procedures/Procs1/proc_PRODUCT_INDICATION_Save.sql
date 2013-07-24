-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_INDICATION_Save]
	@product_indications_PK int = NULL,
	@meddraversion decimal(3,1) = NULL,
	@meddralevel nvarchar(5) = NULL,
	@meddracode int = NULL,
	@name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_INDICATION]
	SET
	[meddraversion] = @meddraversion,
	[meddralevel] = @meddralevel,
	[meddracode] = @meddracode, 
	[name] = @name
	WHERE [product_indications_PK] = @product_indications_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_INDICATION]
		([meddraversion], [meddralevel], [meddracode], [name])
		VALUES
		(@meddraversion, @meddralevel, @meddracode, @name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
