-- Delete
CREATE PROCEDURE  [dbo].[proc_MASTER_FILE_LOCATION_Delete]
	@master_file_location_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MASTER_FILE_LOCATION] WHERE [master_file_location_PK] = @master_file_location_PK
END
