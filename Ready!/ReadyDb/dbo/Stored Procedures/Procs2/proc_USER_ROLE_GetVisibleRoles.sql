-- GeRolesByUserID
CREATE PROCEDURE  [dbo].[proc_USER_ROLE_GetVisibleRoles]
	@UserID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[USER_ROLE]
	
	WHERE [dbo].[USER_ROLE].visible =1

END
