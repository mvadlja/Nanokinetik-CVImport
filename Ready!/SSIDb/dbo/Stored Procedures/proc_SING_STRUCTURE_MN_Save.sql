
-- Save
CREATE PROCEDURE [dbo].[proc_SING_STRUCTURE_MN_Save]
	@sing_structure_mn_PK int = NULL,
	@sing_FK int = NULL,
	@structure_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SING_STRUCTURE_MN]
	SET
	[sing_FK] = @sing_FK,
	[structure_FK] = @structure_FK
	WHERE [sing_structure_mn_PK] = @sing_structure_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SING_STRUCTURE_MN]
		([sing_FK], [structure_FK])
		VALUES
		(@sing_FK, @structure_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
