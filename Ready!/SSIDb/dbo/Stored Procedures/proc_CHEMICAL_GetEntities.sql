
-- GetEntities
CREATE PROCEDURE [dbo].[proc_CHEMICAL_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[chemical_PK], [stoichiometric], [comment], [non_stoichio_FK]
	FROM [dbo].[CHEMICAL]
END
