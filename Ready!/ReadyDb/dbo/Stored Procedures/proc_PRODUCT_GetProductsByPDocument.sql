
create PROCEDURE  [dbo].[proc_PRODUCT_GetProductsByPDocument]
    @document_FK int = NULL,
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*			
        FROM [dbo].PRODUCT_DOCUMENT_MN mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	WHERE 
		(p.product_PK = @product_FK OR @product_FK IS NULL) AND 
		(mn.document_FK = @document_FK OR @document_FK IS NULL)

END