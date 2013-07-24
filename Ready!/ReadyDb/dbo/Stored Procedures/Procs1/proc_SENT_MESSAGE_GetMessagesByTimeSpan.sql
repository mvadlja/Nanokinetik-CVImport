-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_GetMessagesByTimeSpan]
	@StartDate datetime,
	@EndDate datetime
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[SENT_MESSAGE]
	where sent_time >= @StartDate AND sent_time <= @EndDate
	order by [sent_time]
END
