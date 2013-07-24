-- Delete
create PROCEDURE  proc_USER_IN_ROLE_DeleteByUser
	@UserPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER_IN_ROLE] WHERE user_FK = @UserPk AND @UserPk IS NOT NULL
END