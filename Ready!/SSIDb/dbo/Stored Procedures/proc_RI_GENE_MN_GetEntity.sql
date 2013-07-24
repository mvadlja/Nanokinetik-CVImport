
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RI_GENE_MN_GetEntity]
	@ri_gene_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_gene_mn_PK], [ri_FK], [gene_FK]
	FROM [dbo].[RI_GENE_MN]
	WHERE [ri_gene_mn_PK] = @ri_gene_mn_PK
END
