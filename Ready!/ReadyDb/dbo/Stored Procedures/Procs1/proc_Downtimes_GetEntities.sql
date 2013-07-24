-- GetEntities
CREATE PROCEDURE  [dbo].[proc_Downtimes_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDDowntime], [CountryID], [DateFrom], [DateTo], [Comment], [DisplayComment], [Active], [UserShutdowner], [RowVersion]
	FROM [dbo].[Downtimes]
END
