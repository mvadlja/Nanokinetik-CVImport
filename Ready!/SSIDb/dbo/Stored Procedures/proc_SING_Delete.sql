
-- Delete
CREATE PROCEDURE [dbo].[proc_SING_Delete]
	@sing_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SING] WHERE [sing_PK] = @sing_PK
END
