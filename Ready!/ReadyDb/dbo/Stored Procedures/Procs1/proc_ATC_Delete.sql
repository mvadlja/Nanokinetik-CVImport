-- Delete
CREATE PROCEDURE  [dbo].[proc_ATC_Delete]
	@atc_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ATC] WHERE [atc_PK] = @atc_PK
END
