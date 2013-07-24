
-- Delete
CREATE PROCEDURE [dbo].[proc_NON_STOICHIOMETRIC_Delete]
	@non_stoichiometric_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[NON_STOICHIOMETRIC] WHERE [non_stoichiometric_PK] = @non_stoichiometric_PK
END
