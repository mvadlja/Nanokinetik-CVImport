-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_FORM_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [pharmaceutical_form_PK]) AS RowNum,
		[pharmaceutical_form_PK], [name], [ev_code]
		FROM [dbo].[PHARMACEUTICAL_FORM] WHERE deleted='False'
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PHARMACEUTICAL_FORM] WHERE deleted='False'
END
