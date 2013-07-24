
-- GetEntity
CREATE PROCEDURE [dbo].[proc_STRUCTURE_GetEntity]
	@structure_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[structure_PK], [struct_representation], [struct_repres_attach_FK], [optical_activity], [molecular_formula]
	FROM [dbo].[STRUCTURE]
	WHERE [structure_PK] = @structure_PK
END
