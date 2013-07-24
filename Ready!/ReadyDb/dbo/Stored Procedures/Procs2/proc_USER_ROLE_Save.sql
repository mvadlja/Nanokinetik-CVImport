-- Save
CREATE PROCEDURE  proc_USER_ROLE_Save
	@user_role_PK int = NULL,
	@name nvarchar(200) = NULL,
	@display_name nvarchar(500) = NULL,
	@description nvarchar(MAX) = NULL,
	@active bit = NULL,
	@row_version datetime = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[USER_ROLE]
	SET
	[name] = @name,
	[display_name] = @display_name,
	[description] = @description,
	[active] = @active,
	[row_version] = @row_version
	WHERE [user_role_PK] = @user_role_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[USER_ROLE]
		([name], [display_name], [description], [active], [row_version])
		VALUES
		(@name, @display_name, @description, @active, @row_version)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
