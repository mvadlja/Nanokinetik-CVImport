
-- Delete
CREATE PROCEDURE [dbo].[proc_GENE_Delete]
	@gene_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[GENE] WHERE [gene_PK] = @gene_PK
END
