
-- Save
CREATE PROCEDURE [dbo].[proc_SUBTYPE_Save]
	@subtype_PK int = NULL,
	@substance_class_subtype varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBTYPE]
	SET
	[substance_class_subtype] = @substance_class_subtype
	WHERE [subtype_PK] = @subtype_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBTYPE]
		([substance_class_subtype])
		VALUES
		(@substance_class_subtype)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
