-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_Delete]
	@activity_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY] WHERE [activity_PK] = @activity_PK
END
