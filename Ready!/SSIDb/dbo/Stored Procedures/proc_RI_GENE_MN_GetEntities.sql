
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RI_GENE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_gene_mn_PK], [ri_FK], [gene_FK]
	FROM [dbo].[RI_GENE_MN]
END
