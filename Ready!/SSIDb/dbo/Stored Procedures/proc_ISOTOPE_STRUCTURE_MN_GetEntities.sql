
-- GetEntities
CREATE PROCEDURE [dbo].[proc_ISOTOPE_STRUCTURE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[isotope_structure_mn_PK], [isotope_FK], [structure_FK]
	FROM [dbo].[ISOTOPE_STRUCTURE_MN]
END
