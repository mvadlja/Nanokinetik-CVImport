
-- Save
CREATE PROCEDURE [dbo].[proc_NON_STOICHIOMETRIC_Save]
	@non_stoichiometric_PK int = NULL,
	@number_moieties int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[NON_STOICHIOMETRIC]
	SET
	[number_moieties] = @number_moieties
	WHERE [non_stoichiometric_PK] = @non_stoichiometric_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[NON_STOICHIOMETRIC]
		([number_moieties])
		VALUES
		(@number_moieties)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
