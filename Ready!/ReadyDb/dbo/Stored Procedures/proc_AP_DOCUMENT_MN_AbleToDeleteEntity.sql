
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_AbleToDeleteEntity]
	@documentPk int = NULL
AS
DECLARE @NumberOfAuthorisedProducts INT = NULL;

BEGIN
	SET NOCOUNT ON;

	IF (@documentPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumberOfAuthorisedProducts = COUNT(*)
		FROM [dbo].[AP_DOCUMENT_MN] apDocumentMn
		WHERE (apDocumentMn.document_FK = @documentPk OR @documentPk IS NULL)
	
		SET @NumberOfAuthorisedProducts = ISNULL (@NumberOfAuthorisedProducts, 0);

		IF (@NumberOfAuthorisedProducts = 1)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END