-- Save
CREATE PROCEDURE  [dbo].[proc_MEDDRA_Save]
	@meddra_pk int = NULL,
	@version_type_FK int = NULL,
	@level_type_FK int = NULL,
	@code nvarchar(10) = NULL,
	@term nvarchar(256) = NULL,
	@MeddraFullName nvarchar(1000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MEDDRA]
	SET
	[version_type_FK] = @version_type_FK,
	[level_type_FK] = @level_type_FK,
	[code] = @code,
	[term] = @term,
	[MeddraFullName] = @MeddraFullName
	WHERE [meddra_pk] = @meddra_pk

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MEDDRA]
		([version_type_FK], [level_type_FK], [code], [term], [MeddraFullName])
		VALUES
		(@version_type_FK, @level_type_FK, @code, @term, @MeddraFullName)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
