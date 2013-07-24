-- GetEntity
CREATE PROCEDURE  proc_XEVPRM_MESSAGE_GetEntityByMA
	@ma_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_message_PK], [message_number], [message_status_FK], [message_creation_date], [user_FK], [xml], [xml_hash], [sender_ID], [ack], [ack_type], [gateway_submission_date], [gateway_ack_date], [submitted_FK], [generated_file_name], [deleted], [received_message_FK]
	FROM [dbo].[XEVPRM_MESSAGE] 
	WHERE [xevprm_message_PK] = (
		SELECT TOP 1 MA_MA_ENTITY_MN.ma_entity_FK
		FROM MA_MA_ENTITY_MN WHERE 
		MA_MA_ENTITY_MN.ma_FK = @ma_FK AND
		MA_MA_ENTITY_MN.ma_entity_type_FK = 3
		ORDER BY MA_MA_ENTITY_MN.ma_entity_FK DESC
	)
END
