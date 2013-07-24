
-- Delete
CREATE PROCEDURE [dbo].[proc_VERSION_Delete]
	@version_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[VERSION] WHERE [version_PK] = @version_PK
END
