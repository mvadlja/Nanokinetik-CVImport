
-- GetEntity
CREATE PROCEDURE [dbo].[proc_NON_STOICHIOMETRIC_GetEntity]
	@non_stoichiometric_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[non_stoichiometric_PK], [number_moieties]
	FROM [dbo].[NON_STOICHIOMETRIC]
	WHERE [non_stoichiometric_PK] = @non_stoichiometric_PK
END
