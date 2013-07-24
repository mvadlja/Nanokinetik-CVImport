
CREATE PROCEDURE  [dbo].[proc_PRODUCT_AbleToDeleteEntity]
	@ProductPk int = NULL
AS
DECLARE @NumAuthProd INT = NULL;
DECLARE @NumSubUnit INT = NULL;
DECLARE @NumDoc INT = NULL;
BEGIN
	SET NOCOUNT ON;

	IF (@ProductPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumAuthProd = COUNT([dbo].AUTHORISED_PRODUCT.ap_PK)
		FROM [dbo].AUTHORISED_PRODUCT
		WHERE [dbo].AUTHORISED_PRODUCT.product_FK = @ProductPk AND @ProductPk IS NOT NULL

		SELECT @NumSubUnit = COUNT([dbo].PRODUCT_SUB_UNIT_MN.submission_unit_FK)
		FROM dbo.PRODUCT_SUB_UNIT_MN
		WHERE [dbo].PRODUCT_SUB_UNIT_MN.product_FK = @ProductPk AND @ProductPk IS NOT NULL

		SELECT @NumDoc = COUNT([dbo].PRODUCT_DOCUMENT_MN.document_FK)
		FROM dbo.PRODUCT_DOCUMENT_MN
		WHERE [dbo].PRODUCT_DOCUMENT_MN.product_FK = @ProductPk AND @ProductPk IS NOT NULL

		SET @NumAuthProd = ISNULL (@NumAuthProd, 0);
		SET @NumSubUnit = ISNULL (@NumSubUnit, 0);
		SET @NumDoc = ISNULL (@NumDoc, 0);

		IF ((@NumAuthProd + @NumSubUnit + @NumDoc) = 0)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END