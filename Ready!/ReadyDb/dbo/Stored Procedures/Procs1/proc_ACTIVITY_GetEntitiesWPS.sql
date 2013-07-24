-- GetEntitiesWPS
CREATE PROCEDURE  proc_ACTIVITY_GetEntitiesWPS
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'activity_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[activity_PK], [user_FK], [mode_FK], [procedure_type_FK], [name], [description], [comment], [regulatory_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [approval_date], [submission_date], [procedure_number], [legal], [cost], [internal_status_FK], [activity_ID],  [automatic_alerts_on], [dbo].[ACTIVITY].[prevent_start_date_alert],
			[dbo].[ACTIVITY].[prevent_exp_finish_date_alert], [dbo].[ACTIVITY].[billable]
		FROM [dbo].[ACTIVITY]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ACTIVITY]
END
