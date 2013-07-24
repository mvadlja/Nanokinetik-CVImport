-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBSTANCES_Delete]
	@substance_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCES] WHERE [substance_PK] = @substance_PK
END
