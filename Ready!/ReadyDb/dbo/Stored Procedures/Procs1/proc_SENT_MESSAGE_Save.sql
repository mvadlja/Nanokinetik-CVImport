-- Save
CREATE PROCEDURE  [dbo].[proc_SENT_MESSAGE_Save]
	@sent_message_PK int = NULL,
	@msg_data varbinary(MAX) = NULL,
	@sent_time datetime = NULL,
	@msg_type int = NULL,
	@xevmpd_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SENT_MESSAGE]
	SET
	[msg_data] = @msg_data,
	[sent_time] = @sent_time,
	[msg_type] = @msg_type,
	[xevmpd_FK] = @xevmpd_FK
	WHERE [sent_message_PK] = @sent_message_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SENT_MESSAGE]
		([msg_data], [sent_time], [msg_type], [xevmpd_FK])
		VALUES
		(@msg_data, @sent_time, @msg_type, @xevmpd_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
