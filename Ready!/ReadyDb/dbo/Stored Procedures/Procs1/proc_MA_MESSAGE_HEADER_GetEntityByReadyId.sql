-- GetEntity
create PROCEDURE [dbo].[proc_MA_MESSAGE_HEADER_GetEntityByReadyId]
	@ready_id_FK nvarchar(20) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_message_header_PK], [messageformatversion], [messageformatrelease], [registrationnumber], [registrationid], [readymessageid], [messagedateformat], [messagedate], [ready_id_FK], [message_file_name]
	FROM [dbo].[MA_MESSAGE_HEADER]
	WHERE [ready_id_FK] = @ready_id_FK
END
