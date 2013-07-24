
-- Delete
CREATE PROCEDURE [dbo].[proc_REMINDER_STATUS_Delete]
	@reminder_status_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REMINDER_STATUS] WHERE [reminder_status_PK] = @reminder_status_PK
END