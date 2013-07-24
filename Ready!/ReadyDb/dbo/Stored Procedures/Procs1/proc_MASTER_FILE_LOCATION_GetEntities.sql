-- GetEntities
CREATE PROCEDURE  [dbo].[proc_MASTER_FILE_LOCATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[master_file_location_PK], [localnumber], [ev_code], [mflcompany], [mfldepartment], [mflbuilding], [mflstreet], [mflcity], [mflstate], [mflpostcode], [mflcountrycode], [comments]
	FROM [dbo].[MASTER_FILE_LOCATION]
END
