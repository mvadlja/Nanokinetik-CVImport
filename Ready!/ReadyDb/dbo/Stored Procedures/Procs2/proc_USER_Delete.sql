-- Delete
CREATE PROCEDURE  [dbo].[proc_USER_Delete]
	@user_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER] WHERE [user_PK] = @user_PK
END
