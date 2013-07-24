-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_GetMDNDataForTimeStatsByTimeSpan]
	@StartDate datetime,
	@EndDate datetime
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [sentMsg].[sent_time], sentMsg.xevmpd_FK, sentMsg.msg_type as sentType, 
	recMsg.received_time, recMsg.msg_type as receivedType, recMsg.[status] as receivedStatus
	FROM [dbo].[SENT_MESSAGE] sentMsg
	LEFT join [dbo].[RECIEVED_MESSAGE] recMsg ON recMsg.recieved_message_PK =
	 (
         SELECT  TOP 1 recieved_message_PK
         FROM    RECIEVED_MESSAGE
         WHERE   RECIEVED_MESSAGE.xevmpd_FK= sentMsg.xevmpd_FK  AND RECIEVED_MESSAGE.received_time >= sentMsg.sent_time
		 AND RECIEVED_MESSAGE.msg_type = 1
		 ORDER BY RECIEVED_MESSAGE.received_time
         )

	where sent_time >= @StartDate AND sent_time <= @EndDate AND sentMsg.msg_type = 0
	 
	order by [sentMsg].xevmpd_FK
END
