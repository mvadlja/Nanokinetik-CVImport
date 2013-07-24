-- =============================================
-- Author:		<Mateo>
-- Create date: <04.05.2012>
-- Description:	<xEVPRM statistic>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_xEVPRMStatistic]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select 
	(Select Count(DISTINCT [xevmpd_FK]) from [SENT_MESSAGE] where msg_type = 0 and [SENT_MESSAGE].[xevmpd_FK] in 
	(SELECT [XEVPRM_MESSAGE_PK] FROM [XEVPRM_MESSAGE])) as sent_messages,

	(Select Count(*) from XEVPRM_MESSAGE where message_status_FK = 10) as ack_pending, 

	(Select Count(DISTINCT [xevmpd_FK]) from RECIEVED_MESSAGE where msg_type = 0  and [RECIEVED_MESSAGE].[xevmpd_FK] in 
	(SELECT [XEVPRM_MESSAGE_PK] FROM [XEVPRM_MESSAGE])) as received_messages,
	
	(Select Count(DISTINCT [xevmpd_FK]) from RECIEVED_MESSAGE where msg_type = 0 AND processed = '0') as process_pending,
	(Select Count(DISTINCT [xevmpd_FK]) from RECIEVED_MESSAGE where msg_type = 0 and [status]=1 AND is_successfully_processed='1' and [RECIEVED_MESSAGE].[xevmpd_FK] in 
	(SELECT [XEVPRM_MESSAGE_PK] FROM [XEVPRM_MESSAGE] where [XEVPRM_MESSAGE].deleted = 0)) as ACK01,
	(Select Count(DISTINCT [xevmpd_FK]) from RECIEVED_MESSAGE where msg_type = 0 and [status]=2 AND is_successfully_processed='1' and [RECIEVED_MESSAGE].[xevmpd_FK] in 
	(SELECT [XEVPRM_MESSAGE_PK] FROM [XEVPRM_MESSAGE] where [XEVPRM_MESSAGE].deleted = 0)) as ACK02,
	(Select Count(DISTINCT [xevmpd_FK]) from RECIEVED_MESSAGE where msg_type = 0 and [status]=3 AND is_successfully_processed='1' and [RECIEVED_MESSAGE].[xevmpd_FK] in 
	(SELECT [XEVPRM_MESSAGE_PK] FROM [XEVPRM_MESSAGE] where [XEVPRM_MESSAGE].deleted = 0)) as ACK03,
	(Select top 1 [log_time] from SERVICE_LOG where [description] = 'Processing iteration finished' order by service_log_PK desc ) as lastProcessingTime
	
END
