
-- Save
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRESENTATION_TYPE_Save]
	@struct_repres_type_PK int = NULL,
	@name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[STRUCT_REPRESENTATION_TYPE]
	SET
	[name] = @name
	WHERE [struct_repres_type_PK] = @struct_repres_type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[STRUCT_REPRESENTATION_TYPE]
		([name])
		VALUES
		(@name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
