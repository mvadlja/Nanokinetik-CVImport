
-- GetEntity
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_JURISDICTION_GetEntity]
	@jurisdiction_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[jurisdiction_PK], [on_jurisd]
	FROM [dbo].[OFFICIAL_NAME_JURISDICTION]
	WHERE [jurisdiction_PK] = @jurisdiction_PK
END
