-- Delete
CREATE PROCEDURE  [dbo].[proc_REMINDER_Delete]
	@reminder_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REMINDER] WHERE [reminder_PK] = @reminder_PK
END
