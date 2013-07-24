-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[document_PK], [person_FK], [type_FK], [name], [description], [comment], [document_code], [regulatory_status], [version_number], [version_label], [change_date], [effective_start_date], [effective_end_date], [version_date], [localnumber], [version_date_format], [attachment_name], [EDMSBindingRule], [EDMSModifyDate], [EDMSDocumentId], [EDMSVersionNumber], [EDMSDocument]
	FROM [dbo].[DOCUMENT]
END
