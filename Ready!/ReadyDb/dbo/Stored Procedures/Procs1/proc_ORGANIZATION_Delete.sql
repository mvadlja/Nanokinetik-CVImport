-- Delete
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_Delete]
	@organization_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORGANIZATION] WHERE [organization_PK] = @organization_PK
END
