-- GetEntitiesWP
CREATE PROCEDURE  proc_ACTIVITY_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [activity_PK]) AS RowNum,
		[activity_PK], [user_FK], [mode_FK], [procedure_type_FK], [name], [description], [comment], [regulatory_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [approval_date], [submission_date], [procedure_number], [legal], [cost], [internal_status_FK], [activity_ID],  [automatic_alerts_on], [dbo].[ACTIVITY].[prevent_start_date_alert],
			[dbo].[ACTIVITY].[prevent_exp_finish_date_alert], [dbo].[ACTIVITY].[billable]
		FROM [dbo].[ACTIVITY]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ACTIVITY]
END
