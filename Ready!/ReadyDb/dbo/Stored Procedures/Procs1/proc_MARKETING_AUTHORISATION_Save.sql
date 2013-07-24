-- Save
create PROCEDURE  [dbo].[proc_MARKETING_AUTHORISATION_Save]
	@marketing_authorisation_PK int = NULL,
	@ready_id nvarchar(20) = NULL,
	@ma_status_FK int = NULL,
	@message_folder nvarchar(500) = NULL,
	@creation_time datetime = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MARKETING_AUTHORISATION]
	SET
	[ready_id] = @ready_id,
	[ma_status_FK] = @ma_status_FK,
	[message_folder] = @message_folder,
	[creation_time] = @creation_time
	WHERE [marketing_authorisation_PK] = @marketing_authorisation_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MARKETING_AUTHORISATION]
		([ready_id], [ma_status_FK], [message_folder], [creation_time])
		VALUES
		(@ready_id, @ma_status_FK, @message_folder, @creation_time)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
