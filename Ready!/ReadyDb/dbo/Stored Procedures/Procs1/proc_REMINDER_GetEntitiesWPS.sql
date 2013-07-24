-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'reminder_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[reminder_PK], [user_FK], [responsible_user_FK], [table_name], [entity_FK], [related_attribute_name], [related_attribute_value], [reminder_name], [reminder_type], [navigate_url], [time_before_activation], [remind_me_on_email], [additional_emails], [description], [is_automatic], [related_entity_FK], [reminder_user_status_FK], [comment]
		FROM [dbo].[REMINDER]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REMINDER]
END
