-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_GetAttachmentsByAP]
	@ap_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[AP_DOCUMENT_MN].ap_FK, [dbo].[AP_DOCUMENT_MN].ap_document_mn_PK, a.attachment_PK, [dbo].[AP_DOCUMENT_MN].document_FK, disk_file, filetype, attachmentname
	FROM [dbo].[AP_DOCUMENT_MN]
	LEFT JOIN [dbo].[ATTACHMENT] a ON a.document_FK = [dbo].[AP_DOCUMENT_MN].[document_FK]
	LEFT JOIN [dbo].[AUTHORISED_PRODUCT] ap ON ap.ap_PK = [dbo].[AP_DOCUMENT_MN].[ap_FK]
	LEFT JOIN [dbo].DOCUMENT as doc on doc.document_PK=a.document_FK
	WHERE ([dbo].[AP_DOCUMENT_MN].[ap_FK] = @ap_FK OR @ap_FK IS NULL) and doc.type_FK in (Select type_PK from type where name like 'ppi')

END
