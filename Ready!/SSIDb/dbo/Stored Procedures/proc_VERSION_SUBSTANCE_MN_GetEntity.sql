
-- GetEntity
CREATE PROCEDURE [dbo].[proc_VERSION_SUBSTANCE_MN_GetEntity]
	@version_substance_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[version_substance_mn_PK], [version_FK], [substance_FK]
	FROM [dbo].[VERSION_SUBSTANCE_MN]
	WHERE [version_substance_mn_PK] = @version_substance_mn_PK
END
