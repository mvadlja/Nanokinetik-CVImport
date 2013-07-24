
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RS_TRG_MN_GetRSByTRG]
@trg int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT rs.*
	FROM [dbo].RS_TARGET_MN
	left join [dbo].[REFERENCE_SOURCE] rs on rs.reference_source_PK = [dbo].RS_TARGET_MN.rs_FK
	where [dbo].RS_TARGET_MN.target_FK = @trg

END
