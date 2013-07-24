-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_SAVED_SEARCH_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [submission_unit_saved_search_PK]) AS RowNum,
		[submission_unit_saved_search_PK], [product_FK], [activity_FK], [task_FK], [description_type_FK], [agency_FK], [rms_FK], [submission_ID], [s_format_FK], [sequence], [dtd_schema_FK], [dispatch_date_from], [dispatch_date_to], [receipt_date_from], [receipt_to], [displayName], [user_FK], [gridLayout], [isPublic]
		FROM [dbo].[SUBMISSION_UNIT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBMISSION_UNIT_SAVED_SEARCH]
END
