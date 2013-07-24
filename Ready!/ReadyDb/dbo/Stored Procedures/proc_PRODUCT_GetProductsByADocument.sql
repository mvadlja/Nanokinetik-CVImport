
create PROCEDURE  [dbo].[proc_PRODUCT_GetProductsByADocument]
    @document_FK int = NULL,
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*			
    FROM [dbo].ACTIVITY_DOCUMENT_MN mn
    LEFT JOIN [dbo].ACTIVITY a ON a.activity_PK = mn.activity_FK
    LEFT JOIN [dbo].DOCUMENT d ON d.document_PK = mn.document_FK
	LEFT JOIN [dbo].ACTIVITY_PRODUCT_MN apMN ON apMN.activity_FK = a.activity_PK
	LEFT JOIN [dbo].PRODUCT p ON p.product_PK = apMN.product_FK
	WHERE 
		(a.activity_PK = @activity_FK OR @activity_FK IS NULL) AND 
		(mn.document_FK = @document_FK OR @document_FK IS NULL)

END