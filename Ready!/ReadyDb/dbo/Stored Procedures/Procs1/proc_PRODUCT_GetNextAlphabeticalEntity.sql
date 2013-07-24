-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetNextAlphabeticalEntity]
	@product_PK int = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
	[product_PK], ProductName
	FROM [dbo].[PRODUCT] as product
	WHERE (ProductName > (SELECT ProductName 
						FROM [PRODUCT]
						WHERE [product_PK]=@product_PK)) 
				OR (ProductName=(SELECT ProductName 
								FROM [PRODUCT]
								WHERE [product_PK]=@product_PK) AND [product_PK] > @product_PK)
	ORDER BY ProductName, [product_PK]
	
END
