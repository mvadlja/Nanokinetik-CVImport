-- Save
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_Save]
	@ap_document_mn_PK int = NULL,
	@document_FK int = NULL,
	@ap_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AP_DOCUMENT_MN]
	SET
	[document_FK] = @document_FK,
	[ap_FK] = @ap_FK
	WHERE [ap_document_mn_PK] = @ap_document_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AP_DOCUMENT_MN]
		([document_FK], [ap_FK])
		VALUES
		(@document_FK, @ap_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
