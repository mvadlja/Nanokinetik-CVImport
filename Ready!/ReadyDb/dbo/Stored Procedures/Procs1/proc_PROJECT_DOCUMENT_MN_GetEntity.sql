-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_GetEntity]
	@project_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[project_document_PK], [project_FK], [document_FK]
	FROM [dbo].[PROJECT_DOCUMENT_MN]
	WHERE [project_document_PK] = @project_document_PK
END
