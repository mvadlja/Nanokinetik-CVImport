
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SING_STRUCTURE_MN_GetEntity]
	@sing_structure_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sing_structure_mn_PK], [sing_FK], [structure_FK]
	FROM [dbo].[SING_STRUCTURE_MN]
	WHERE [sing_structure_mn_PK] = @sing_structure_mn_PK
END
