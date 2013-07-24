-- Delete
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_NAME_Delete]
	@time_unit_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TIME_UNIT_NAME] WHERE [time_unit_name_PK] = @time_unit_name_PK
END
