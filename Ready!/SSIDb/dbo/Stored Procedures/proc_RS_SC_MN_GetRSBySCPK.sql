
-- GetEntities
create PROCEDURE [dbo].[proc_RS_SC_MN_GetRSBySCPK]
@SCPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.*
	FROM [dbo].[RS_SC_MN]
	left join [dbo].[REFERENCE_SOURCE] rs on rs.reference_source_PK = [dbo].[RS_SC_MN].rs_FK
	where [dbo].[RS_SC_MN].sc_FK = @SCPK

END
