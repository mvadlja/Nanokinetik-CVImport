-- GetEntitiesWP
create PROCEDURE [dbo].[proc_MA_EVENT_TYPE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ma_event_type_PK]) AS RowNum,
		[ma_event_type_PK], [name], [enum_name], [ma_event_type_severity_FK]
		FROM [dbo].[MA_EVENT_TYPE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MA_EVENT_TYPE]
END
