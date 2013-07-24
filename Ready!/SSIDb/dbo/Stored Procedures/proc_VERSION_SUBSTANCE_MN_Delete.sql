
-- Delete
CREATE PROCEDURE [dbo].[proc_VERSION_SUBSTANCE_MN_Delete]
	@version_substance_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[VERSION_SUBSTANCE_MN] WHERE [version_substance_mn_PK] = @version_substance_mn_PK
END
