-- GetEntity
CREATE PROCEDURE  [dbo].[proc_MASTER_FILE_LOCATION_GetEntity]
	@master_file_location_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[master_file_location_PK], [localnumber], [ev_code], [mflcompany], [mfldepartment], [mflbuilding], [mflstreet], [mflcity], [mflstate], [mflpostcode], [mflcountrycode], [comments]
	FROM [dbo].[MASTER_FILE_LOCATION]
	WHERE [master_file_location_PK] = @master_file_location_PK
END
