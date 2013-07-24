-- Delete
CREATE PROCEDURE  [dbo].[proc_Downtimes_Delete]
	@IDDowntime int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[Downtimes] WHERE [IDDowntime] = @IDDowntime
END
