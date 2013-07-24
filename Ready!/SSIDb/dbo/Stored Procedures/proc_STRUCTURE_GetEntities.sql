
-- GetEntities
CREATE PROCEDURE [dbo].[proc_STRUCTURE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[structure_PK], [struct_representation], [struct_repres_attach_FK], [optical_activity], [molecular_formula]
	FROM [dbo].[STRUCTURE]
END
