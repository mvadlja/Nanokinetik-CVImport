
-- GetEntities
create PROCEDURE [dbo].[proc_SING_STRUCTURE_MN_GetStructBySingPK]
@SingPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT struct.*
	FROM [dbo].[SING_STRUCTURE_MN]
	left join [dbo].[STRUCTURE] struct on struct.structure_PK = [dbo].[SING_STRUCTURE_MN].structure_FK
	where [dbo].[SING_STRUCTURE_MN].sing_FK = @SingPK
END
