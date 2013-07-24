
-- GetEntities
create PROCEDURE [dbo].[proc_RS_GENE_MN_GetRSByGenePK]
@GenePK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.*
	FROM [dbo].[RS_GENE_MN]
	left join [dbo].[REFERENCE_SOURCE] rs on rs.reference_source_PK = [dbo].[RS_GENE_MN].rs_FK
	where [dbo].[RS_GENE_MN].gene_FK = @GenePK

END
