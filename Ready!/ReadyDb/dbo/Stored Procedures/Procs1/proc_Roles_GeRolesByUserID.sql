-- GeRolesByUserID
CREATE PROCEDURE  [dbo].[proc_Roles_GeRolesByUserID]
	@UserID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[Roles].[IDRole], [dbo].[Roles].[Name], [dbo].[Roles].[DisplayName], [dbo].[Roles].[Description], [dbo].[Roles].[Active], [dbo].[Roles].[RowVersion]
	FROM [dbo].[Roles]
	LEFT JOIN [dbo].[UsersInRoles] ON Roles.IDRole = UsersInRoles.RoleID
	WHERE [dbo].[UsersInRoles].[UserID] = @UserID

END
