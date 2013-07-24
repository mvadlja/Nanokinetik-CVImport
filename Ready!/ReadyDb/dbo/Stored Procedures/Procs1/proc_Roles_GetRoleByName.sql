-- GetRoleByName
CREATE PROCEDURE  [dbo].[proc_Roles_GetRoleByName]
	@Name nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[Roles].[IDRole], [dbo].[Roles].[Name], [dbo].[Roles].[DisplayName], [dbo].[Roles].[Description], [dbo].[Roles].[Active], [dbo].[Roles].[RowVersion]
	FROM [dbo].[Roles]
	WHERE [dbo].[Roles].[Name] = @Name

END
