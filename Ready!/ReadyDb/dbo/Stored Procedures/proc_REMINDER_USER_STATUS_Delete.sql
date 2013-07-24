
-- Delete
CREATE PROCEDURE [dbo].[proc_REMINDER_USER_STATUS_Delete]
	@reminder_user_status_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REMINDER_USER_STATUS] WHERE [reminder_user_status_PK] = @reminder_user_status_PK
END