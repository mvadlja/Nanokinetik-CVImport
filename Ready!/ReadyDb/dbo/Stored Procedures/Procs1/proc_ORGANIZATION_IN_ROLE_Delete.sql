-- Delete
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_Delete]
	@organization_in_role_ID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORGANIZATION_IN_ROLE] WHERE [organization_in_role_ID] = @organization_in_role_ID
END
