-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [project_PK]) AS RowNum,
		[project_PK], [user_FK], [name], [comment], [start_date], [expected_finished_date], [actual_finished_date], [description], [internal_status_type_FK], [automatic_alerts_on],[prevent_start_date_alert], [prevent_exp_finish_date_alert]
		FROM [dbo].[PROJECT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PROJECT]
END
