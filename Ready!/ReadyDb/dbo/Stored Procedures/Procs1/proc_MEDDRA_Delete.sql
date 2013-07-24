-- Delete
CREATE PROCEDURE  [dbo].[proc_MEDDRA_Delete]
	@meddra_pk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MEDDRA] WHERE [meddra_pk] = @meddra_pk
END
