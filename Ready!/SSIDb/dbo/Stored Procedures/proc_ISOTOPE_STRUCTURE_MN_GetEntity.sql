
-- GetEntity
CREATE PROCEDURE [dbo].[proc_ISOTOPE_STRUCTURE_MN_GetEntity]
	@isotope_structure_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[isotope_structure_mn_PK], [isotope_FK], [structure_FK]
	FROM [dbo].[ISOTOPE_STRUCTURE_MN]
	WHERE [isotope_structure_mn_PK] = @isotope_structure_mn_PK
END
