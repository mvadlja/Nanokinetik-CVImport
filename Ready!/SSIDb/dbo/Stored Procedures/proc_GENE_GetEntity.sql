
-- GetEntity
CREATE PROCEDURE [dbo].[proc_GENE_GetEntity]
	@gene_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[gene_PK], [gene_sequence_origin], [gene_id], [gene_name]
	FROM [dbo].[GENE]
	WHERE [gene_PK] = @gene_PK
END
