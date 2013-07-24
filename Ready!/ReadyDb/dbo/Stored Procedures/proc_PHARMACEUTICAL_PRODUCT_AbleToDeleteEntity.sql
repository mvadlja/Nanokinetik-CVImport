
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_AbleToDeleteEntity]
		@PharmaceuticalProductPk int = NULL
AS
DECLARE @numberOfDocuments INT = 0;
DECLARE @NumberOfProducts INT = NULL;
BEGIN
	SET NOCOUNT ON;

	IF (@PharmaceuticalProductPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumberOfProducts = COUNT([dbo].PRODUCT_PP_MN.product_FK)
		FROM [dbo].PRODUCT_PP_MN
		WHERE [dbo].PRODUCT_PP_MN.pp_FK = @PharmaceuticalProductPk AND @PharmaceuticalProductPk IS NOT NULL

		SELECT @numberOfDocuments = COUNT([dbo].[PP_DOCUMENT_MN].pp_document_PK)
		FROM [dbo].[PP_DOCUMENT_MN] 
		WHERE ([dbo].[PP_DOCUMENT_MN].pp_FK = @PharmaceuticalProductPk AND @PharmaceuticalProductPk IS NOT NULL)

		SET @NumberOfProducts = ISNULL (@NumberOfProducts, 0);
		SET @numberOfDocuments = ISNULL (@numberOfDocuments, 0);

		IF ((@NumberOfProducts + @numberOfDocuments) = 0)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END