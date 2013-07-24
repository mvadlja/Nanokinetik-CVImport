-- Delete
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_SAVED_SEARCH_Delete]
	@time_unit_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TIME_UNIT_SAVED_SEARCH] WHERE [time_unit_saved_search_PK] = @time_unit_saved_search_PK
END
