-- Save
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ENTITY_TYPE_Save]
	@xevprm_entity_type_PK int = NULL,
	@name nvarchar(50) = NULL,
	@table_name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[XEVPRM_ENTITY_TYPE]
	SET
	[name] = @name,
	[table_name] = @table_name
	WHERE [xevprm_entity_type_PK] = @xevprm_entity_type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[XEVPRM_ENTITY_TYPE]
		([name], [table_name])
		VALUES
		(@name, @table_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
