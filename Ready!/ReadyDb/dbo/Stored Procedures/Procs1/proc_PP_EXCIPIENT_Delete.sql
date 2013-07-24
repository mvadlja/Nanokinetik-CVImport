-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_EXCIPIENT_Delete]
	@excipient_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_EXCIPIENT] WHERE [excipient_PK] = @excipient_PK
END
