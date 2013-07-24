
-- GetEntity
CREATE PROCEDURE [dbo].[proc_GENE_ELEMENT_GetEntity]
	@gene_element_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[gene_element_PK], [ge_type], [ge_id], [ge_name]
	FROM [dbo].[GENE_ELEMENT]
	WHERE [gene_element_PK] = @gene_element_PK
END
