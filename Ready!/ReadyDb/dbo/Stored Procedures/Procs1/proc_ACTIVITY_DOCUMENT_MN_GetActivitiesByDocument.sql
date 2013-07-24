-- GetActivitiesByDocument
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_GetActivitiesByDocument]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.activity_document_PK, d.document_PK, a.activity_PK, a.name
	FROM [dbo].[ACTIVITY_DOCUMENT_MN] mn
	LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = mn.document_FK
	LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = mn.activity_FK
	WHERE (mn.document_FK = @document_FK OR @document_FK IS NULL)

END
