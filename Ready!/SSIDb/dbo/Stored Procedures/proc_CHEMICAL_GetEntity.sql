
-- GetEntity
CREATE PROCEDURE [dbo].[proc_CHEMICAL_GetEntity]
	@chemical_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[chemical_PK], [stoichiometric], [comment], [non_stoichio_FK]
	FROM [dbo].[CHEMICAL]
	WHERE [chemical_PK] = @chemical_PK
END
