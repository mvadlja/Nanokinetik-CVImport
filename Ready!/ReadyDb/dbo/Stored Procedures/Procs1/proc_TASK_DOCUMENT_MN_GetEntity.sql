-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TASK_DOCUMENT_MN_GetEntity]
	@task_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_document_PK], [task_FK], [document_FK]
	FROM [dbo].[TASK_DOCUMENT_MN]
	WHERE [task_document_PK] = @task_document_PK
END
