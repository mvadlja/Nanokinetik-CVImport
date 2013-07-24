
-- Delete
CREATE PROCEDURE [dbo].[proc_STRUCTURE_Delete]
	@structure_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[STRUCTURE] WHERE [structure_PK] = @structure_PK
END
