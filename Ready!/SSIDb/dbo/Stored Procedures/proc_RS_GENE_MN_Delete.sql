
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_GENE_MN_Delete]
	@rs_gene_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_GENE_MN] WHERE [rs_gene_mn_PK] = @rs_gene_mn_PK
END
