-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_Downtimes_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [IDDowntime]) AS RowNum,
		[IDDowntime], [CountryID], [DateFrom], [DateTo], [Comment], [DisplayComment], [Active], [UserShutdowner], [RowVersion]
		FROM [dbo].[Downtimes]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[Downtimes]
END
