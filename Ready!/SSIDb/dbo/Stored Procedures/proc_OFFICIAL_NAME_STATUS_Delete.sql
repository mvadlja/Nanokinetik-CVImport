
-- Delete
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_STATUS_Delete]
	@official_name_status_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[OFFICIAL_NAME_STATUS] WHERE [official_name_status_PK] = @official_name_status_PK
END
