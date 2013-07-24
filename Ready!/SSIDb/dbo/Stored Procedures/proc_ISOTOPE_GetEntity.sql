
-- GetEntity
CREATE PROCEDURE [dbo].[proc_ISOTOPE_GetEntity]
	@isotope_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[isotope_PK], [nuclide_id], [nuclide_name], [substitution_type]
	FROM [dbo].[ISOTOPE]
	WHERE [isotope_PK] = @isotope_PK
END
