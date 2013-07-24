
-- Delete
CREATE PROCEDURE [dbo].[proc_SING_STRUCTURE_MN_Delete]
	@sing_structure_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SING_STRUCTURE_MN] WHERE [sing_structure_mn_PK] = @sing_structure_mn_PK
END
