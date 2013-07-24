-- GetEntities
CREATE PROCEDURE [dbo].[proc_LOCATION_GetUserPermissions]
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
	JOIN dbo.USER_ROLE r ON ura.user_role_FK = r.user_role_PK
	where u.username = @Username AND r.active = '1' AND ua.active = '1' AND l.active = '1'
END