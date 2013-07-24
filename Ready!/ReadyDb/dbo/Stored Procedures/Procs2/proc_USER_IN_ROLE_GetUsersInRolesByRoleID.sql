-- GetUsersInRolesByRoleID
CREATE PROCEDURE  [dbo].[proc_USER_IN_ROLE_GetUsersInRolesByRoleID]
	@RoleID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[USER_IN_ROLE].[user_in_role_PK], [dbo].[USER_IN_ROLE].[user_FK], [dbo].[USER_IN_ROLE].[user_role_FK], [dbo].[USER_IN_ROLE].[row_version]
	FROM [dbo].[USER_IN_ROLE]
	WHERE [dbo].[USER_IN_ROLE].[user_role_FK] = @RoleID

END
