
-- GetEntities
create PROCEDURE [dbo].[proc_GENE_ELEMENT_GetGEByRIPK]
@RIPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ge.*
	FROM [dbo].[RI_GE_MN]
	left join [dbo].[GENE_ELEMENT] ge on ge.gene_element_PK = [dbo].[RI_GE_MN].ge_FK
	where [dbo].[RI_GE_MN].ri_FK = @RIPK
	
END
