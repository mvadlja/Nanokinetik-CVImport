-- Delete
CREATE PROCEDURE  [dbo].[proc_USER_ROLE_Delete]
	@user_role_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER_ROLE] WHERE [user_role_PK] = @user_role_PK
END
