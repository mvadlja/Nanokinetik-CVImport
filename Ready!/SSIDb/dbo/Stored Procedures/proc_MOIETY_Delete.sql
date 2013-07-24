
-- Delete
CREATE PROCEDURE [dbo].[proc_MOIETY_Delete]
	@moiety_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MOIETY] WHERE [moiety_PK] = @moiety_PK
END
