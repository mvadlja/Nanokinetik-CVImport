-- GetUsersInRolesByUserID
CREATE PROCEDURE proc_USER_IN_ROLE_GetUsersInRolesByUserID
	@UserID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT userInRole.[user_in_role_PK], userInRole.[user_FK], userInRole.[user_role_FK], userInRole.[row_version]
	FROM [dbo].[USER_IN_ROLE] userInRole
	LEFT JOIN USER_ROLE userRole ON userRole.user_role_PK = userInRole.user_role_FK
	WHERE userInRole.[user_FK] = @UserID AND userRole.visible = '1'

END
