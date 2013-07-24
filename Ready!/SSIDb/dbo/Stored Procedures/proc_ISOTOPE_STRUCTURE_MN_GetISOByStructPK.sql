
-- GetEntities
create PROCEDURE [dbo].[proc_ISOTOPE_STRUCTURE_MN_GetISOByStructPK]
@StructPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT iso.*
	FROM [dbo].[ISOTOPE_STRUCTURE_MN]
	left join [dbo].[ISOTOPE] iso on iso.isotope_PK = [dbo].[ISOTOPE_STRUCTURE_MN].isotope_FK
	where [dbo].[ISOTOPE_STRUCTURE_MN].structure_FK = @StructPK
END
