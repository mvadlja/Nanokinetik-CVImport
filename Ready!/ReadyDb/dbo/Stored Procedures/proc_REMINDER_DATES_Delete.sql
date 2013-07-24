
-- Delete
CREATE PROCEDURE [dbo].[proc_REMINDER_DATES_Delete]
	@reminder_date_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REMINDER_DATES] WHERE [reminder_date_PK] = @reminder_date_PK
END