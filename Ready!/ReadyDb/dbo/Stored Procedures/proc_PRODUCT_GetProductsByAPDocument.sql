
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetProductsByAPDocument]
    @document_FK int = NULL,
	@ap_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*			
    FROM [dbo].AP_DOCUMENT_MN mn
	LEFT JOIN [dbo].AUTHORISED_PRODUCT ap ON ap.ap_PK = mn.ap_FK
	LEFT JOIN [dbo].PRODUCT p ON p.product_PK = ap.product_FK
	WHERE 
		(ap.ap_PK = @ap_FK OR @ap_FK IS NULL) AND 
		(mn.document_FK = @document_FK OR @document_FK IS NULL)

END