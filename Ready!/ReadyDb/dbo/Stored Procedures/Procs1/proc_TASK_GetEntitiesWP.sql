-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_TASK_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [task_PK]) AS RowNum,
		[task_PK], [activity_FK], [user_FK], [task_name_FK], [description], [comment], [type_internal_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [POM_internal_status],  [automatic_alerts_on],[prevent_start_date_alert], [prevent_exp_finish_date_alert], [task_ID]
		FROM [dbo].[TASK]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TASK]
END
