-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_DOCUMENT_MN_Delete]
	@pp_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_DOCUMENT_MN] WHERE [pp_document_PK] = @pp_document_PK
END
