-- GetEntities
CREATE PROCEDURE  [dbo].[proc_USER_ROLE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_role_PK], [name], [display_name], [description], [active], [row_version]
	FROM [dbo].[USER_ROLE]
END
