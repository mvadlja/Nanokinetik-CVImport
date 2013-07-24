
-- GetEntity
CREATE PROCEDURE [dbo].[proc_VERSION_GetEntity]
	@version_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[version_PK], [version_number], [effectve_date], [change_made]
	FROM [dbo].[VERSION]
	WHERE [version_PK] = @version_PK
END
