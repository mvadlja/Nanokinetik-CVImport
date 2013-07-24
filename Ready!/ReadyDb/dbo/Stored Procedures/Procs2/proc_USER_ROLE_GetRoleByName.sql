-- GetRoleByName
CREATE PROCEDURE  [dbo].[proc_USER_ROLE_GetRoleByName]
	@Name nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[USER_ROLE].[user_role_PK], [dbo].[USER_ROLE].[name], [dbo].[USER_ROLE].[display_name], [dbo].[USER_ROLE].[description], [dbo].[USER_ROLE].[active], [dbo].[USER_ROLE].[row_version]
	FROM [dbo].[USER_ROLE]
	WHERE [dbo].[USER_ROLE].[name] = @Name

END
