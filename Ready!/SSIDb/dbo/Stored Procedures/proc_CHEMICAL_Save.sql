
-- Save
CREATE PROCEDURE [dbo].[proc_CHEMICAL_Save]
	@chemical_PK int = NULL,
	@stoichiometric varbinary(1) = NULL,
	@comment varchar(4000) = NULL,
	@non_stoichio_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[CHEMICAL]
	SET
	[stoichiometric] = @stoichiometric,
	[comment] = @comment,
	[non_stoichio_FK] = @non_stoichio_FK
	WHERE [chemical_PK] = @chemical_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[CHEMICAL]
		([stoichiometric], [comment], [non_stoichio_FK])
		VALUES
		(@stoichiometric, @comment, @non_stoichio_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
