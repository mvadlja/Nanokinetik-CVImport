-- Save
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_Save]
	@activity_document_PK int = NULL,
	@activity_FK int = NULL,
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY_DOCUMENT_MN]
	SET
	[activity_FK] = @activity_FK,
	[document_FK] = @document_FK
	WHERE [activity_document_PK] = @activity_document_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY_DOCUMENT_MN]
		([activity_FK], [document_FK])
		VALUES
		(@activity_FK, @document_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
