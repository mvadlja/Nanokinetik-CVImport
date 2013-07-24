
-- Delete
CREATE PROCEDURE [dbo].[proc_RI_GENE_MN_Delete]
	@ri_gene_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RI_GENE_MN] WHERE [ri_gene_mn_PK] = @ri_gene_mn_PK
END
