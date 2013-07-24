-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ap_document_mn_PK], [document_FK], [ap_FK]
	FROM [dbo].[AP_DOCUMENT_MN]
END
