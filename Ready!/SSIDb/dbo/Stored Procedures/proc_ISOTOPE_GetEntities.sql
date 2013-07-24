
-- GetEntities
CREATE PROCEDURE [dbo].[proc_ISOTOPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[isotope_PK], [nuclide_id], [nuclide_name], [substitution_type]
	FROM [dbo].[ISOTOPE]
END
