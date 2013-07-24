-- Delete
create PROCEDURE  proc_USER_IN_ROLE_DeleteByUserRole
	@UserRolePk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER_IN_ROLE] WHERE user_role_FK = @UserRolePk AND @UserRolePk IS NOT NULL
END