
-- Delete
CREATE PROCEDURE [dbo].[proc_REMINDER_REPEATING_MODES_Delete]
	@reminder_repeating_mode_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REMINDER_REPEATING_MODES] WHERE [reminder_repeating_mode_PK] = @reminder_repeating_mode_PK
END