
-- GetEntities
create PROCEDURE [dbo].[proc_RS_SN_MN_GetRSBySNPK]
@SNPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.*
	FROM [dbo].[RS_SN_MN]
	left join [dbo].[REFERENCE_SOURCE] rs on rs.reference_source_PK = [dbo].[RS_SN_MN].rs_FK
	where [dbo].[RS_SN_MN].substance_name_FK = @SNPK

END
