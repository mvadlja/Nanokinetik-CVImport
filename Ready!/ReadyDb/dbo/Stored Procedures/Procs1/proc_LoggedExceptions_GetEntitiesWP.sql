-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_LoggedExceptions_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [IDLoggedException]) AS RowNum,
		[IDLoggedException], [Username], [ExceptionType], [ExceptionMessage], [TargetSite], [StackTrace], [Source], [Date], [ServerName], [UniqueKey]
		FROM [dbo].[LoggedExceptions]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[LoggedExceptions]
END
