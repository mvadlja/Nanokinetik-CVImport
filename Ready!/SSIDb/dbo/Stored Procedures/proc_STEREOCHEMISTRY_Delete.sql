
-- Delete
CREATE PROCEDURE [dbo].[proc_STEREOCHEMISTRY_Delete]
	@stereochemistry_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[STEREOCHEMISTRY] WHERE [stereochemistry_PK] = @stereochemistry_PK
END
