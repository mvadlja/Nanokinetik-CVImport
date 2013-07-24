-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_document_mn_PK], [product_FK], [document_FK]
	FROM [dbo].[PRODUCT_DOCUMENT_MN]
END
