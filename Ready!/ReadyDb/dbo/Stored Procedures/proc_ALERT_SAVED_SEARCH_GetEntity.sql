
-- GetEntity
CREATE PROCEDURE [dbo].[proc_ALERT_SAVED_SEARCH_GetEntity]
	@alert_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[alert_saved_search_PK], [product_FK], [ap_FK], [project_FK], [activity_FK], [task_FK], [document_FK], [gridLayout], [isPublic], [name], [reminder_repeating_mode_FK], [send_mail], [displayName], [user_FK]
	FROM [dbo].[ALERT_SAVED_SEARCH]
	WHERE [alert_saved_search_PK] = @alert_saved_search_PK
END