-- Delete
CREATE PROCEDURE  [dbo].[proc_TASK_DOCUMENT_MN_Delete]
	@task_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TASK_DOCUMENT_MN] WHERE [task_document_PK] = @task_document_PK
END
