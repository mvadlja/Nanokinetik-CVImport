-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_SAVED_SEARCH_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'document_saved_search_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[document_saved_search_PK], [product_FK], [ap_FK], [project_FK], [activity_FK], [task_FK], [name], [type_FK], [version_number], [version_label], [document_number], [person_FK], [regulatory_status], [change_date_from], [change_date_to], [effective_start_date_from], [effective_start_date_to], [effective_end_date_from], [effective_end_date_to], [displayName], [user_FK1], [gridLayout], [isPublic], [pp_FK], [version_date_from], [version_date_to], [ev_code], [content], [language_code], [comments]
		FROM [dbo].[DOCUMENT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[DOCUMENT_SAVED_SEARCH]
END
