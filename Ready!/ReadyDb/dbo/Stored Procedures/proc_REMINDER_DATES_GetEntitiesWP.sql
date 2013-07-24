
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_REMINDER_DATES_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [reminder_date_PK]) AS RowNum,
		[reminder_date_PK], [reminder_date], [reminder_repeating_mode_FK], [reminder_status_FK], [reminder_FK] 
		FROM [dbo].[REMINDER_DATES]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REMINDER_DATES]
END