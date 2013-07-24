-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_SAVED_SEARCH_GetEntity]
	@time_unit_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[time_unit_saved_search_PK], [activity_FK], [time_unit_FK], [user_FK], [actual_date_from], [actual_date_to], [displayName], [user_FK1], [gridLayout], [isPublic]
	FROM [dbo].[TIME_UNIT_SAVED_SEARCH]
	WHERE [time_unit_saved_search_PK] = @time_unit_saved_search_PK
END
