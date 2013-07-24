
-- Delete
CREATE PROCEDURE [dbo].[proc_ISOTOPE_Delete]
	@isotope_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ISOTOPE] WHERE [isotope_PK] = @isotope_PK
END
