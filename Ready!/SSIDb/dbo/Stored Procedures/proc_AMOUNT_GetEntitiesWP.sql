
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_AMOUNT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [amount_PK]) AS RowNum,
		[amount_PK], [quantity], [lownumvalue], [lownumunit], [lownumprefix], [lowdenomvalue], [lowdenomunit], [lowdenomprefix], [highnumvalue], [highnumunit], [highnumprefix], [highdenomvalue], [highdenomunit], [highdenomprefix], [average], [prefix], [unit], [nonnumericvalue]
		FROM [dbo].[AMOUNT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AMOUNT]
END
