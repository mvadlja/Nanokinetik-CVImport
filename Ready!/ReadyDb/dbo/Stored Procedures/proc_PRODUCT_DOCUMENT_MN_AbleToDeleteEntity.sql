
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_AbleToDeleteEntity]
	@documentPk int = NULL
AS
DECLARE @NumberOfProducts INT = NULL;

BEGIN
	SET NOCOUNT ON;

	IF (@documentPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumberOfProducts = COUNT(*)
		FROM [dbo].[PRODUCT_DOCUMENT_MN] productDocumentMn
		WHERE (productDocumentMn.document_FK = @documentPk OR @documentPk IS NULL)
	
		SET @NumberOfProducts = ISNULL (@NumberOfProducts, 0);

		IF (@NumberOfProducts = 1)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END