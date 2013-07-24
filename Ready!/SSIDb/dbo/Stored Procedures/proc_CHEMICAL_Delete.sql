
-- Delete
CREATE PROCEDURE [dbo].[proc_CHEMICAL_Delete]
	@chemical_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[CHEMICAL] WHERE [chemical_PK] = @chemical_PK
END
