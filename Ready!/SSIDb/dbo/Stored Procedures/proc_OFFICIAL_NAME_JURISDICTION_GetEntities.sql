
-- GetEntities
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_JURISDICTION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[jurisdiction_PK], [on_jurisd]
	FROM [dbo].[OFFICIAL_NAME_JURISDICTION]
END
