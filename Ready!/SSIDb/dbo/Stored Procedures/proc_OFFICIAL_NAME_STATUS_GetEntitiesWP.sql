
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_STATUS_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [official_name_status_PK]) AS RowNum,
		[official_name_status_PK], [status_name]
		FROM [dbo].[OFFICIAL_NAME_STATUS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[OFFICIAL_NAME_STATUS]
END
