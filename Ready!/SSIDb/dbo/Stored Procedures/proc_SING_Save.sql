
-- Save
CREATE PROCEDURE [dbo].[proc_SING_Save]
	@sing_PK int = NULL,
	@chemical_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SING]
	SET
	[chemical_FK] = @chemical_FK
	WHERE [sing_PK] = @sing_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SING]
		([chemical_FK])
		VALUES
		(@chemical_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
