-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_GetEntity]
	@ap_document_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ap_document_mn_PK], [document_FK], [ap_FK]
	FROM [dbo].[AP_DOCUMENT_MN]
	WHERE [ap_document_mn_PK] = @ap_document_mn_PK
END
