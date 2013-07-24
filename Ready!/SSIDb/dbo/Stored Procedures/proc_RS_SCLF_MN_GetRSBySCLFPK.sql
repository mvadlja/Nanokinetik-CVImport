
-- GetEntities
create PROCEDURE [dbo].[proc_RS_SCLF_MN_GetRSBySCLFPK]
@SCLFPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.*
	FROM [dbo].[RS_SCLF_MN]
	left join [dbo].[REFERENCE_SOURCE] rs on rs.reference_source_PK = [dbo].[RS_SCLF_MN].rs_FK
	where [dbo].[RS_SCLF_MN].sclf_FK = @SCLFPK

END
