
-- Delete
CREATE PROCEDURE [dbo].[proc_ISOTOPE_STRUCTURE_MN_Delete]
	@isotope_structure_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ISOTOPE_STRUCTURE_MN] WHERE [isotope_structure_mn_PK] = @isotope_structure_mn_PK
END
