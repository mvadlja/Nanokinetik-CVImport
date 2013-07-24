-- Delete
create PROCEDURE  proc_AP_DOCUMENT_MN_DeleteByAuthorisedProduct
	@AuthorisedProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AP_DOCUMENT_MN] WHERE ap_FK = @AuthorisedProductPk AND @AuthorisedProductPk IS NOT NULL
END