-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_SAVED_SEARCH_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'submission_unit_saved_search_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[submission_unit_saved_search_PK], [product_FK], [activity_FK], [task_FK], [description_type_FK], [agency_FK], [rms_FK], [submission_ID], [s_format_FK], [sequence], [dtd_schema_FK], [dispatch_date_from], [dispatch_date_to], [receipt_date_from], [receipt_to], [displayName], [user_FK], [gridLayout], [isPublic]
		FROM [dbo].[SUBMISSION_UNIT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBMISSION_UNIT_SAVED_SEARCH]
END
