-- GetEntities
create PROCEDURE proc_USER_ROLE_ACTION_GetEntitiesByUserRole
	@UserRolePk int = NULL
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_role_action_PK], [user_role_FK], [location_FK], [user_action_FK]
	FROM [dbo].[USER_ROLE_ACTION]
	where [user_role_FK] = @UserRolePk AND @UserRolePk IS NOT NULL
END