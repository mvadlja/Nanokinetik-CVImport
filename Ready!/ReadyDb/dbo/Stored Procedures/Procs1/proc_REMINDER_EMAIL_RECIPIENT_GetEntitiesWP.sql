-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_REMINDER_EMAIL_RECIPIENT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [reminder_email_recipient_PK]) AS RowNum,
		[reminder_email_recipient_PK], [reminder_FK], [person_FK]
		FROM [dbo].[REMINDER_EMAIL_RECIPIENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REMINDER_EMAIL_RECIPIENT]
END
