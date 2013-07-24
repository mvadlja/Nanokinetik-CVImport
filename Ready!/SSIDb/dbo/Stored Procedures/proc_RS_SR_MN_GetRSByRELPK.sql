
-- GetEntities
create PROCEDURE [dbo].[proc_RS_SR_MN_GetRSByRELPK]
@RELPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.*
	FROM [dbo].[RS_SR_MN]
	left join [dbo].[REFERENCE_SOURCE] rs on rs.reference_source_PK = [dbo].[RS_SR_MN].rs_FK
	where [dbo].[RS_SR_MN].sr_FK = @RELPK

END
