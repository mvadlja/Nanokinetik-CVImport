
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RS_GENE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_gene_mn_PK], [rs_FK], [gene_FK]
	FROM [dbo].[RS_GENE_MN]
END
