
-- GetEntities
create PROCEDURE [dbo].[proc_GENE_GetGeneByRIPK]
@RIPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT gene.*
	FROM [dbo].[RI_GENE_MN]
	left join [dbo].[GENE] gene on gene.gene_PK = [dbo].[RI_GENE_MN].gene_FK
	where [dbo].[RI_GENE_MN].ri_FK = @RIPK
	
END
