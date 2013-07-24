-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[project_document_PK], [project_FK], [document_FK]
	FROM [dbo].[PROJECT_DOCUMENT_MN]
END
