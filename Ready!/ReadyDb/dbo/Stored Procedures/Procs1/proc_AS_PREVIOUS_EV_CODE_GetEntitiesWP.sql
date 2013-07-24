-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_AS_PREVIOUS_EV_CODE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [as_previous_ev_code_PK]) AS RowNum,
		[as_previous_ev_code_PK], [devevcode]
		FROM [dbo].[AS_PREVIOUS_EV_CODE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AS_PREVIOUS_EV_CODE]
END
