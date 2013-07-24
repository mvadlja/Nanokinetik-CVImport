-- GetEntitiesByCountryID
CREATE PROCEDURE  [dbo].[proc_Downtimes_GetEntitiesByCountryID]
	@CountryID int = NULL,
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [IDDowntime]) AS RowNum,
		DistinctDowntimes.* FROM
		(
			SELECT DISTINCT
			[dbo].[Downtimes].[IDDowntime], [dbo].[Downtimes].[CountryID], [dbo].[Downtimes].[DateFrom], [dbo].[Downtimes].[DateTo], [dbo].[Downtimes].[Comment], [dbo].[Downtimes].[DisplayComment], [dbo].[Downtimes].[Active], [dbo].[Downtimes].[UserShutdowner], [dbo].[Downtimes].[RowVersion]
			FROM [dbo].[Downtimes]
			WHERE [dbo].[Downtimes].[CountryID] = @CountryID
		) DistinctDowntimes
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(DISTINCT [dbo].[Downtimes].[IDDowntime]) FROM [dbo].[Downtimes]
	WHERE [dbo].[Downtimes].[CountryID] = @CountryID

END
