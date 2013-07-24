
-- Delete
CREATE PROCEDURE [dbo].[proc_ALERT_SAVED_SEARCH_Delete]
	@alert_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ALERT_SAVED_SEARCH] WHERE [alert_saved_search_PK] = @alert_saved_search_PK
END