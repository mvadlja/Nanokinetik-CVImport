
-- GetEntities
CREATE PROCEDURE [dbo].[proc_GENE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[gene_PK], [gene_sequence_origin], [gene_id], [gene_name]
	FROM [dbo].[GENE]
END
