
-- GetEntities
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_STATUS_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[official_name_status_PK], [status_name]
	FROM [dbo].[OFFICIAL_NAME_STATUS]
END
