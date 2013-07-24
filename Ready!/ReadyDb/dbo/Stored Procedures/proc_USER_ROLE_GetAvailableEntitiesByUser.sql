-- GetActiveRolesByUserID
create PROCEDURE  proc_USER_ROLE_GetAvailableEntitiesByUser
	@UserPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.*
	FROM [dbo].[USER_ROLE] r
	WHERE r.user_role_PK NOT IN 
	(
		SELECT urMn.user_role_FK
		FROM [dbo].[USER_IN_ROLE] urMn
		WHERE urmn.user_FK = @UserPk AND @UserPk IS NOT NULL
	)
END