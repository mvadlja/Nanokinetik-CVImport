-- Save
CREATE PROCEDURE  [dbo].[proc_USER_IN_ROLE_Save]
	@user_in_role_PK int = NULL,
	@user_FK int = NULL,
	@user_role_FK int = NULL,
	@row_version datetime = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[USER_IN_ROLE]
	SET
	[user_FK] = @user_FK,
	[user_role_FK] = @user_role_FK,
	[row_version] = @row_version
	WHERE [user_in_role_PK] = @user_in_role_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[USER_IN_ROLE]
		([user_FK], [user_role_FK], [row_version])
		VALUES
		(@user_FK, @user_role_FK, @row_version)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
