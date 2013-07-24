-- Delete
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_Delete]
	@time_unit_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TIME_UNIT] WHERE [time_unit_PK] = @time_unit_PK
END
