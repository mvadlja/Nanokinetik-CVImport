-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_MASTER_FILE_LOCATION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [master_file_location_PK]) AS RowNum,
		[master_file_location_PK], [localnumber], [ev_code], [mflcompany], [mfldepartment], [mflbuilding], [mflstreet], [mflcity], [mflstate], [mflpostcode], [mflcountrycode], [comments]
		FROM [dbo].[MASTER_FILE_LOCATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MASTER_FILE_LOCATION]
END
