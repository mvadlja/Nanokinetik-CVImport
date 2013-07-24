-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [reminder_PK]) AS RowNum,
		[reminder_PK], [user_FK], [responsible_user_FK], [table_name], [entity_FK], [related_attribute_name], [related_attribute_value], [reminder_name], [reminder_type], [navigate_url], [time_before_activation], [remind_me_on_email], [additional_emails], [description], [is_automatic], [related_entity_FK], [reminder_user_status_FK], [comment]
		FROM [dbo].[REMINDER]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REMINDER]
END
