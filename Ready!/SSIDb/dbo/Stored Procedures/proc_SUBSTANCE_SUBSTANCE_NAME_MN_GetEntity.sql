
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_NAME_MN_GetEntity]
	@substance_substance_name_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_substance_name_mn_PK], [substance_FK], [substance_name_FK]
	FROM [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN]
	WHERE [substance_substance_name_mn_PK] = @substance_substance_name_mn_PK
END
