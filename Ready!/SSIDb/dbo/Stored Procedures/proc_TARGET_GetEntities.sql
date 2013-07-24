
-- GetEntities
CREATE PROCEDURE [dbo].[proc_TARGET_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[target_PK], [target_gene_id], [target_gene_name], [interaction_type], [target_organism_type], [target_type]
	FROM [dbo].[TARGET]
END
