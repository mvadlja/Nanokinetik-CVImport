-- GetEntities
create PROCEDURE proc_USER_GetUserPermissions
	@Username nvarchar(200) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT l.*, ua.unique_name AS Permission
	FROM dbo.[USER] u
	JOIN dbo.USER_IN_ROLE urMn ON urMn.user_FK = u.user_PK
	JOIN dbo.USER_ROLE_ACTION ura ON ura.user_role_FK = urMn.user_role_FK
	JOIN dbo.LOCATION l ON l.location_PK = ura.location_FK
	JOIN dbo.USER_ACTION ua ON ua.user_action_PK = ura.user_action_FK
	where u.username = @Username
	--ORDER BY l.location_url
END