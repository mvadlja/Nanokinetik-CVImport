
-- GetEntity
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_STATUS_GetEntity]
	@official_name_status_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[official_name_status_PK], [status_name]
	FROM [dbo].[OFFICIAL_NAME_STATUS]
	WHERE [official_name_status_PK] = @official_name_status_PK
END
