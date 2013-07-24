
-- Delete
CREATE PROCEDURE [dbo].[proc_TARGET_Delete]
	@target_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TARGET] WHERE [target_PK] = @target_PK
END
