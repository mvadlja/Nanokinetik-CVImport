
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_REMINDER_STATUS_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [reminder_status_PK]) AS RowNum,
		[reminder_status_PK], [name], [enum_name]
		FROM [dbo].[REMINDER_STATUS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REMINDER_STATUS]
END