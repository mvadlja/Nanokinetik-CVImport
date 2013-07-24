
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_REMINDER_REPEATING_MODES_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [reminder_repeating_mode_PK]) AS RowNum,
		[reminder_repeating_mode_PK], [name], [enum_name]
		FROM [dbo].[REMINDER_REPEATING_MODES]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REMINDER_REPEATING_MODES]
END