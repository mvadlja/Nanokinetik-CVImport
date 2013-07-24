-- Save
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_LANGUAGE_MN_Save]
	@document_language_mn_PK int = NULL,
	@document_FK int = NULL,
	@language_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[DOCUMENT_LANGUAGE_MN]
	SET
	[document_FK] = @document_FK,
	[language_FK] = @language_FK
	WHERE [document_language_mn_PK] = @document_language_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[DOCUMENT_LANGUAGE_MN]
		([document_FK], [language_FK])
		VALUES
		(@document_FK, @language_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
