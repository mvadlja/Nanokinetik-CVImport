
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_Save]
	@substance_name_PK int = NULL,
	@subst_name varchar(4000) = NULL,
	@subst_name_type_FK int = NULL,
	@language_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_NAME]
	SET
	[subst_name] = @subst_name,
	[subst_name_type_FK] = @subst_name_type_FK,
	[language_FK] = @language_FK
	WHERE [substance_name_PK] = @substance_name_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_NAME]
		([subst_name], [subst_name_type_FK], [language_FK])
		VALUES
		(@subst_name, @subst_name_type_FK, @language_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
