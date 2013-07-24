-- GetEntity
CREATE PROCEDURE  [dbo].[proc_Downtimes_GetEntity]
	@IDDowntime int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDDowntime], [CountryID], [DateFrom], [DateTo], [Comment], [DisplayComment], [Active], [UserShutdowner], [RowVersion]
	FROM [dbo].[Downtimes]
	WHERE [IDDowntime] = @IDDowntime
END
