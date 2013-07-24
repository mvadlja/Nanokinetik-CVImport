-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetDocumentsByProduct]
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

		SELECT d.document_PK, d.person_FK, d.type_FK, d.name, d.description, d.comment, d.document_code, d.regulatory_status, d.version_number, d.version_label, d.change_date, d.effective_start_date, d.effective_end_date, d.version_date, d.localnumber, d.version_date_format, d.attachment_name,d.attachment_type_FK, d.[EDMSBindingRule], d.[EDMSModifyDate], d.[EDMSDocumentId], d.[EDMSVersionNumber]
		FROM [dbo].[PRODUCT_DOCUMENT_MN]
		LEFT JOIN [dbo].[DOCUMENT] d ON d.document_PK = [dbo].[PRODUCT_DOCUMENT_MN].[document_FK]
		LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = [dbo].[PRODUCT_DOCUMENT_MN].[product_FK]
		WHERE ([dbo].[PRODUCT_DOCUMENT_MN].[product_FK] = @product_FK OR @product_FK IS NULL)

END
