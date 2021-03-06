﻿-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_MASTER_FILE_LOCATION_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'master_file_location_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[master_file_location_PK], [localnumber], [ev_code], [mflcompany], [mfldepartment], [mflbuilding], [mflstreet], [mflcity], [mflstate], [mflpostcode], [mflcountrycode], [comments]
		FROM [dbo].[MASTER_FILE_LOCATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MASTER_FILE_LOCATION]
END
