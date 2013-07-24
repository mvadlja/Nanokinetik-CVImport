-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PP_DOCUMENT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pp_document_PK], [pp_FK], [doc_FK]
	FROM [dbo].[PP_DOCUMENT_MN]
END
