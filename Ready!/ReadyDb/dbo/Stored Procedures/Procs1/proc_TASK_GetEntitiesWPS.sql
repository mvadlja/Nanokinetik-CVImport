-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_TASK_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'task_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[task_PK], [activity_FK], [user_FK], [task_name_FK], [description], [comment], [type_internal_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [POM_internal_status],  [automatic_alerts_on],[prevent_start_date_alert], [prevent_exp_finish_date_alert], [task_ID]
		FROM [dbo].[TASK]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TASK]
END
