-- Save
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_SaveLinkToDocument]
	@attachment_PK int = NULL,
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ATTACHMENT]
	SET
	[document_FK] = @document_FK
	WHERE [attachment_PK] = @attachment_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ATTACHMENT]
		([document_FK])
		VALUES
		(@document_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
