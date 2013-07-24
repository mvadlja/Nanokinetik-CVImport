
-- GetEntitiesWPS
CREATE PROCEDURE [dbo].[proc_ALERT_SAVED_SEARCH_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'alert_saved_search_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[alert_saved_search_PK], [product_FK], [ap_FK], [project_FK], [activity_FK], [task_FK], [document_FK], [gridLayout], [isPublic], [name], [reminder_repeating_mode_FK], [send_mail], [displayName], [user_FK]
		FROM [dbo].[ALERT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ALERT_SAVED_SEARCH]
END