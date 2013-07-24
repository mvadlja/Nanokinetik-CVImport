﻿-- Save
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_TRANSLATION_Save]
	@substance_alias_translation_PK int = NULL,
	@languagecode nvarchar(2) = NULL,
	@term ntext = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_ALIAS_TRANSLATION]
	SET
	[languagecode] = @languagecode,
	[term] = @term
	WHERE [substance_alias_translation_PK] = @substance_alias_translation_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_ALIAS_TRANSLATION]
		([languagecode], [term])
		VALUES
		(@languagecode, @term)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
