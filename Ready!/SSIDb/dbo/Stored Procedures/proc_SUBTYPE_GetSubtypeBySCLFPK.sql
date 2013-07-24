
-- GetEntities
create PROCEDURE [dbo].[proc_SUBTYPE_GetSubtypeBySCLFPK]
@SCLFPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT subtype.*
	FROM [dbo].[SUBTYPE_SCLF_MN]
	left join [dbo].[SUBTYPE] subtype on subtype.subtype_PK = [dbo].[SUBTYPE_SCLF_MN].subtype_FK
	where [dbo].[SUBTYPE_SCLF_MN].sclf_FK = @SCLFPK
	
END
