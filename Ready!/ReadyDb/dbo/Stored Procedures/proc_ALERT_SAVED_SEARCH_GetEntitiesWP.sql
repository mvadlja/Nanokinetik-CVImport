
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_ALERT_SAVED_SEARCH_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [alert_saved_search_PK]) AS RowNum,
		[alert_saved_search_PK], [product_FK], [ap_FK], [project_FK], [activity_FK], [task_FK], [document_FK], [gridLayout], [isPublic], [name], [reminder_repeating_mode_FK], [send_mail], [displayName], [user_FK]
		FROM [dbo].[ALERT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ALERT_SAVED_SEARCH]
END