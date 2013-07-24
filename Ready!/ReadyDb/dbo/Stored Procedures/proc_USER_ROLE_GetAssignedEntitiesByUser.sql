-- GetActiveRolesByUserID
create PROCEDURE  proc_USER_ROLE_GetAssignedEntitiesByUser
	@UserPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.*
	FROM [dbo].[USER_IN_ROLE] urMn
	JOIN [dbo].[USER_ROLE] r ON r.[user_role_PK] = urmn.[user_role_FK]
	WHERE urmn.user_FK = @UserPk AND @UserPk IS NOT NULL
END