-- GetActiveRolesByUsername
CREATE PROCEDURE  [dbo].[proc_USER_ROLE_GetActiveRolesByUsername]
	@UserName nvarchar(20) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[USER_ROLE].[user_role_PK], [dbo].[USER_ROLE].[name], [dbo].[USER_ROLE].[display_name], [dbo].[USER_ROLE].[description], [dbo].[USER_ROLE].[active], [dbo].[USER_ROLE].[row_version]
	FROM [dbo].[USER_ROLE]
	LEFT JOIN [dbo].[USER_IN_ROLE] ON [USER_ROLE].[user_role_PK] = [USER_IN_ROLE].[user_role_FK]
	LEFT JOIN [dbo].[USER] ON [USER_IN_ROLE].[user_FK] = [USER].[user_PK]
	WHERE [dbo].[USER].[username] = @UserName
	AND [USER_ROLE].active = 1

END
