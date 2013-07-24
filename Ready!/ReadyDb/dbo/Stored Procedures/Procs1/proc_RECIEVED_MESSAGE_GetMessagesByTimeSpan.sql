-- GetEntities
CREATE PROCEDURE  [dbo].[proc_RECIEVED_MESSAGE_GetMessagesByTimeSpan]
	@StartDate datetime,
	@EndDate datetime
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[RECIEVED_MESSAGE]
	where received_time >= @StartDate AND received_time <= @EndDate
	order by [received_time]
END
