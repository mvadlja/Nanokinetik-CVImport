
-- GetEntities
CREATE PROCEDURE [dbo].[proc_NON_STOICHIOMETRIC_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[non_stoichiometric_PK], [number_moieties]
	FROM [dbo].[NON_STOICHIOMETRIC]
END
