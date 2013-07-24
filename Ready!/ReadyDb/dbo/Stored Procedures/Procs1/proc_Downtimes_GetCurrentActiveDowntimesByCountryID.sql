-- GetCurrentActiveDowntimesByCountryID
CREATE PROCEDURE  [dbo].[proc_Downtimes_GetCurrentActiveDowntimesByCountryID]
	@CountryID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[Downtimes].[IDDowntime], [dbo].[Downtimes].[CountryID], [dbo].[Downtimes].[DateFrom], [dbo].[Downtimes].[DateTo], [dbo].[Downtimes].[Comment], [dbo].[Downtimes].[DisplayComment], [dbo].[Downtimes].[Active], [dbo].[Downtimes].[UserShutdowner], [dbo].[Downtimes].[RowVersion]
	FROM [dbo].[Downtimes]
	WHERE Downtimes.CountryID = @CountryID
	AND GETDATE() BETWEEN Downtimes.DateFrom AND ISNULL(Downtimes.DateTo, DATEADD(YEAR,1, GETDATE()))
	AND Downtimes.Active = 1
END
