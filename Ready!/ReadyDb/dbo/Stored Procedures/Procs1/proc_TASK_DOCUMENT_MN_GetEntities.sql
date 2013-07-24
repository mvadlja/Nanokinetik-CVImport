-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TASK_DOCUMENT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_document_PK], [task_FK], [document_FK]
	FROM [dbo].[TASK_DOCUMENT_MN]
END
