
CREATE PROCEDURE  [dbo].[proc_TASK_DOCUMENT_MN_AbleToDeleteEntity]
	@documentPk int = NULL
AS
DECLARE @NumberOfTasks INT = NULL;

BEGIN
	SET NOCOUNT ON;

	IF (@documentPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumberOfTasks = COUNT(*)
		FROM [dbo].[TASK_DOCUMENT_MN] taskDocumentMn
		WHERE (taskDocumentMn.document_FK = @documentPk OR @documentPk IS NULL)
	
		SET @NumberOfTasks = ISNULL (@NumberOfTasks, 0);

		IF (@NumberOfTasks = 1)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END