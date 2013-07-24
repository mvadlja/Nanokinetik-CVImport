-- Save
create PROCEDURE  [dbo].[proc_MA_MESSAGE_HEADER_Save]
	@ma_message_header_PK int = NULL,
	@messageformatversion nvarchar(10) = NULL,
	@messageformatrelease nvarchar(10) = NULL,
	@registrationnumber nvarchar(30) = NULL,
	@registrationid bigint = NULL,
	@readymessageid nvarchar(32) = NULL,
	@messagedateformat nvarchar(10) = NULL,
	@messagedate datetime = NULL,
	@ready_id_FK nvarchar(32) = NULL,
	@message_file_name nvarchar(1000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MA_MESSAGE_HEADER]
	SET
	[messageformatversion] = @messageformatversion,
	[messageformatrelease] = @messageformatrelease,
	[registrationnumber] = @registrationnumber,
	[registrationid] = @registrationid,
	[readymessageid] = @readymessageid,
	[messagedateformat] = @messagedateformat,
	[messagedate] = @messagedate,
	[ready_id_FK] = @ready_id_FK,
	[message_file_name] = @message_file_name
	WHERE [ma_message_header_PK] = @ma_message_header_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MA_MESSAGE_HEADER]
		([messageformatversion], [messageformatrelease], [registrationnumber], [registrationid], [readymessageid], [messagedateformat], [messagedate], [ready_id_FK], [message_file_name])
		VALUES
		(@messageformatversion, @messageformatrelease, @registrationnumber, @registrationid, @readymessageid, @messagedateformat, @messagedate, @ready_id_FK, @message_file_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
