-- Save
CREATE PROCEDURE  proc_XEVPRM_MESSAGE_Save
	@xevprm_message_PK int = NULL,
	@message_number nvarchar(100) = NULL,
	@message_status_FK int = NULL,
	@message_creation_date datetime = NULL,
	@user_FK int = NULL,
	@xml nvarchar(MAX) = NULL,
	@xml_hash nvarchar(50) = NULL,
	@sender_ID nvarchar(60) = NULL,
	@ack nvarchar(MAX) = NULL,
	@ack_type int = NULL,
	@gateway_submission_date datetime = NULL,
	@gateway_ack_date datetime = NULL,
	@submitted_FK int = NULL,
	@generated_file_name nvarchar(MAX) = NULL,
	@deleted bit = NULL,
	@received_message_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[XEVPRM_MESSAGE]
	SET
	[message_number] = @message_number,
	[message_status_FK] = @message_status_FK,
	[message_creation_date] = @message_creation_date,
	[user_FK] = @user_FK,
	[xml] = @xml,
	[xml_hash] = @xml_hash,
	[sender_ID] = @sender_ID,
	[ack] = @ack,
	[ack_type] = @ack_type,
	[gateway_submission_date] = @gateway_submission_date,
	[gateway_ack_date] = @gateway_ack_date,
	[submitted_FK] = @submitted_FK,
	[generated_file_name] = @generated_file_name,
	[deleted] = @deleted,
	[received_message_FK] = @received_message_FK
	WHERE [xevprm_message_PK] = @xevprm_message_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[XEVPRM_MESSAGE]
		([message_number], [message_status_FK], [message_creation_date], [user_FK], [xml], [xml_hash], [sender_ID], [ack], [ack_type], [gateway_submission_date], [gateway_ack_date], [submitted_FK], [generated_file_name], [deleted], [received_message_FK])
		VALUES
		(@message_number, @message_status_FK, @message_creation_date, @user_FK, @xml, @xml_hash, @sender_ID, @ack, @ack_type, @gateway_submission_date, @gateway_ack_date, @submitted_FK, @generated_file_name, @deleted, @received_message_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
