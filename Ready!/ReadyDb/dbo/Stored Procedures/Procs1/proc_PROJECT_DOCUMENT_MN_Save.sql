-- Save
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_Save]
	@project_document_PK int = NULL,
	@project_FK int = NULL,
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PROJECT_DOCUMENT_MN]
	SET
	[project_FK] = @project_FK,
	[document_FK] = @document_FK
	WHERE [project_document_PK] = @project_document_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PROJECT_DOCUMENT_MN]
		([project_FK], [document_FK])
		VALUES
		(@project_FK, @document_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
