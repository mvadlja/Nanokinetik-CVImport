-- GetActiveRolesByUsername
CREATE PROCEDURE  [dbo].[proc_Roles_GetActiveRolesByUsername]
	@UserName nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[Roles].[IDRole], [dbo].[Roles].[Name], [dbo].[Roles].[DisplayName], [dbo].[Roles].[Description], [dbo].[Roles].[Active], [dbo].[Roles].[RowVersion]
	FROM [dbo].[Roles]
	LEFT JOIN [dbo].[UsersInRoles] ON Roles.IDRole = UsersInRoles.RoleID
	LEFT JOIN [dbo].[Users] ON UsersInRoles.UserID = Users.UserID
	WHERE [dbo].[Users].[UserName] = @UserName
	AND Roles.Active = 1

END
