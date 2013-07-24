-- GetEntity
CREATE PROCEDURE  [dbo].[proc_USER_ROLE_GetEntity]
	@user_role_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_role_PK], [name], [display_name], [description], [active], [row_version]
	FROM [dbo].[USER_ROLE]
	WHERE [user_role_PK] = @user_role_PK
END
