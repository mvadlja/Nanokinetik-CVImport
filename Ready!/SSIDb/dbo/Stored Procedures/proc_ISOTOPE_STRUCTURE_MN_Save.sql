
-- Save
CREATE PROCEDURE [dbo].[proc_ISOTOPE_STRUCTURE_MN_Save]
	@isotope_structure_mn_PK int = NULL,
	@isotope_FK int = NULL,
	@structure_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ISOTOPE_STRUCTURE_MN]
	SET
	[isotope_FK] = @isotope_FK,
	[structure_FK] = @structure_FK
	WHERE [isotope_structure_mn_PK] = @isotope_structure_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ISOTOPE_STRUCTURE_MN]
		([isotope_FK], [structure_FK])
		VALUES
		(@isotope_FK, @structure_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
