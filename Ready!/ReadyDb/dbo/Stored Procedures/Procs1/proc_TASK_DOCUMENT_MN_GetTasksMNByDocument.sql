-- GetActivitiesMNByDocument
CREATE PROCEDURE  [dbo].[proc_TASK_DOCUMENT_MN_GetTasksMNByDocument]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[TASK_DOCUMENT_MN] mn
	WHERE (mn.document_FK = @document_FK OR @document_FK IS NULL)

END
