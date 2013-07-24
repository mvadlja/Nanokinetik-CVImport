-- Delete
create PROCEDURE [dbo].[proc_NOTIFICATION_TYPE_Delete]
	@notification_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[NOTIFICATION_TYPE] WHERE [notification_type_PK] = @notification_type_PK
END
