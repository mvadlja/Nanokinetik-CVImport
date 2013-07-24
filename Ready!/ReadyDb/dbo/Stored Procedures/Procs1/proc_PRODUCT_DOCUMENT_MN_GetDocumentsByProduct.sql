-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_GetDocumentsByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.document_PK, d.change_date, d.[comment], d.[description], d.document_code, d.effective_end_date, d.effective_start_date, d.name AS DocumentName, p.givenname, p.familyname, d.regulatory_status, t.name AS TypeName, t.[group], d.version_label, d.version_number
	FROM [dbo].[PRODUCT_DOCUMENT_MN]
	LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = [dbo].[PRODUCT_DOCUMENT_MN].[document_FK]
	LEFT JOIN [dbo].[AUTHORISED_PRODUCT] ap ON ap.ap_PK = [dbo].[PRODUCT_DOCUMENT_MN].product_FK
	LEFT JOIN [dbo].[PERSON] p ON p.person_PK = d.person_FK
	LEFT JOIN [dbo].[TYPE] t ON t.type_PK = d.type_FK
	WHERE ([dbo].[PRODUCT_DOCUMENT_MN].product_FK = @Product_FK OR @Product_FK IS NULL)

END
