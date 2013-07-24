-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_SAVED_SEARCH_Delete]
	@submission_unit_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBMISSION_UNIT_SAVED_SEARCH] WHERE [submission_unit_saved_search_PK] = @submission_unit_saved_search_PK
END
