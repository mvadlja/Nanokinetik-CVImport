
-- GetEntities
create PROCEDURE [dbo].[proc_RS_GE_MN_GetRSBySNPK]
@GEPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.*
	FROM [dbo].[RS_GE_MN]
	left join [dbo].[REFERENCE_SOURCE] rs on rs.reference_source_PK = [dbo].[RS_GE_MN].rs_FK
	where [dbo].[RS_GE_MN].ge_FK = @GEPK

END
