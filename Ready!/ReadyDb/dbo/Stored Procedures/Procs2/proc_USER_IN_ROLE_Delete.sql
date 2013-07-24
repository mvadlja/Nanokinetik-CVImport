-- Delete
CREATE PROCEDURE  [dbo].[proc_USER_IN_ROLE_Delete]
	@user_in_role_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER_IN_ROLE] WHERE [user_in_role_PK] = @user_in_role_PK
END
