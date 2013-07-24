
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_AbleToDeleteEntity]
	@documentPk int = NULL
AS
DECLARE @NumberOfProjects INT = NULL;

BEGIN
	SET NOCOUNT ON;

	IF (@documentPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumberOfProjects = COUNT(*)
		FROM [dbo].[PROJECT_DOCUMENT_MN] projectDocumentMn
		WHERE (projectDocumentMn.document_FK = @documentPk OR @documentPk IS NULL)
	
		SET @NumberOfProjects = ISNULL (@NumberOfProjects, 0);

		IF (@NumberOfProjects = 1)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END