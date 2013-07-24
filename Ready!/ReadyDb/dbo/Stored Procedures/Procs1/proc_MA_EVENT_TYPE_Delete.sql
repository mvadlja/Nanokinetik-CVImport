-- Delete
create PROCEDURE [dbo].[proc_MA_EVENT_TYPE_Delete]
	@ma_event_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MA_EVENT_TYPE] WHERE [ma_event_type_PK] = @ma_event_type_PK
END
