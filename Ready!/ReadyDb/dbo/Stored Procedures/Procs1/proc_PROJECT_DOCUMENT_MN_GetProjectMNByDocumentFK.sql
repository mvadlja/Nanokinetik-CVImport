-- GetActivitiesMNByDocument
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_GetProjectMNByDocumentFK]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[PROJECT_DOCUMENT_MN] mn
	WHERE (mn.document_FK = @document_FK OR @document_FK IS NULL)

END
