-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'project_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;
	 
	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[project_PK], [user_FK], [name], [comment], [start_date], [expected_finished_date], [actual_finished_date], [description], [internal_status_type_FK],  [automatic_alerts_on],[prevent_start_date_alert], [prevent_exp_finish_date_alert]
		FROM [dbo].[PROJECT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PROJECT]
END
