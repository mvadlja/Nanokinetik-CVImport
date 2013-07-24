-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_GetDocumentsByAP]
	@ap_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.document_PK, d.change_date, d.[comment], d.[description], d.document_code, d.effective_end_date, d.effective_start_date, d.name AS DocumentName, p.givenname, p.familyname, d.regulatory_status, t.name AS TypeName, t.[group], d.version_label, d.version_number
	FROM [dbo].[AP_DOCUMENT_MN]
	LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = [dbo].[AP_DOCUMENT_MN].[document_FK]
	LEFT JOIN [dbo].[AUTHORISED_PRODUCT] ap ON ap.ap_PK = [dbo].[AP_DOCUMENT_MN].[ap_FK]
	LEFT JOIN [dbo].[PERSON] p ON p.person_PK = d.person_FK
	LEFT JOIN [dbo].[TYPE] t ON t.type_PK = d.type_FK
	WHERE ([dbo].[AP_DOCUMENT_MN].[ap_FK] = @ap_FK OR @ap_FK IS NULL)

END
