﻿-- GetEntity
CREATE PROCEDURE  proc_XEVPRM_MESSAGE_GetEntity
	@xevprm_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_message_PK], [message_number], [message_status_FK], [message_creation_date], [user_FK], [xml], [xml_hash], [sender_ID], [ack], [ack_type], [gateway_submission_date], [gateway_ack_date], [submitted_FK], [generated_file_name], [deleted], [received_message_FK]
	FROM [dbo].[XEVPRM_MESSAGE]
	WHERE [xevprm_message_PK] = @xevprm_message_PK
END
