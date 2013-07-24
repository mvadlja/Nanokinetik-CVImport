-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetDocumentsByAP]
	@ap_FK int = NULL,
	@documentTypeName nvarchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT d.document_PK, d.person_FK, d.type_FK, d.name, d.description, d.comment, d.document_code, d.regulatory_status, d.version_number, d.version_label, d.change_date, d.effective_start_date, d.effective_end_date, d.version_date, d.localnumber, d.version_date_format, d.attachment_name, d.attachment_type_FK, d.[EDMSBindingRule], d.[EDMSModifyDate], d.[EDMSDocumentId], d.[EDMSVersionNumber], d.[EDMSDocument]
	FROM [dbo].[AP_DOCUMENT_MN]
	LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = [dbo].[AP_DOCUMENT_MN].[document_FK]
	LEFT JOIN [dbo].[AUTHORISED_PRODUCT] ap ON ap.ap_PK = [dbo].[AP_DOCUMENT_MN].[ap_FK]
	LEFT JOIN [dbo].[TYPE] t on t.type_PK = d.type_FK
	WHERE ([dbo].[AP_DOCUMENT_MN].[ap_FK] = @ap_FK OR @ap_FK IS NULL)
	AND (LOWER(t.name) = LOWER(@documentTypeName) OR @documentTypeName IS NULL)
	
END
