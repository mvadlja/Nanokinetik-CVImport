-- Save
CREATE PROCEDURE  [dbo].[proc_TASK_DOCUMENT_MN_Save]
	@task_document_PK int = NULL,
	@task_FK int = NULL,
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TASK_DOCUMENT_MN]
	SET
	[task_FK] = @task_FK,
	[document_FK] = @document_FK
	WHERE [task_document_PK] = @task_document_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TASK_DOCUMENT_MN]
		([task_FK], [document_FK])
		VALUES
		(@task_FK, @document_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
