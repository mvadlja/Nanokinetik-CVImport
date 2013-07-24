
-- GetEntities
CREATE PROCEDURE [dbo].[proc_GENE_ELEMENT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[gene_element_PK], [ge_type], [ge_id], [ge_name]
	FROM [dbo].[GENE_ELEMENT]
END
