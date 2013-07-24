-- GetDocumentsByActivity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_GetDocumentsByActivity]
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.document_PK, d.change_date, d.[comment], d.[description], d.document_code, d.effective_end_date, d.effective_start_date, d.name AS DocumentName, p.givenname, p.familyname, d.regulatory_status, t.name AS TypeName, t.[group], d.version_label, d.version_number
	FROM [dbo].[ACTIVITY_DOCUMENT_MN] mn
	LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = mn.document_FK
	LEFT JOIN [dbo].ACTIVITY a ON a.activity_PK = mn.activity_FK
	LEFT JOIN [dbo].[PERSON] p ON p.person_PK = d.person_FK
	LEFT JOIN [dbo].[TYPE] t ON t.type_PK = d.type_FK
	WHERE (mn.activity_FK = @activity_FK OR @activity_FK IS NULL)

END
