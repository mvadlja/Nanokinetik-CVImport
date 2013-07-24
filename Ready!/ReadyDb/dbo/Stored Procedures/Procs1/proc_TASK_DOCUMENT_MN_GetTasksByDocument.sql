-- GetActivitiesByDocument
CREATE PROCEDURE  [dbo].[proc_TASK_DOCUMENT_MN_GetTasksByDocument]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.task_document_PK, d.document_PK, t.task_PK, tname.task_name as name
	FROM [dbo].TASK_DOCUMENT_MN mn
	LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = mn.document_FK
	LEFT JOIN [dbo].[TASK] t ON t.task_PK = mn.task_FK
	LEFT JOIN [dbo].[TASK_NAME] tname ON tname.task_name_PK = t.task_name_FK
	WHERE (mn.document_FK = @document_FK OR @document_FK IS NULL)

END
