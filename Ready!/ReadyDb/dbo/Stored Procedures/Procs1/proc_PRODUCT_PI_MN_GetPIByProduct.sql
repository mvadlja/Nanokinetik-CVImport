-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_GetPIByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.product_pi_mn_PK, p.product_PK, pin.product_indications_PK, pin.meddracode, pin.meddralevel, pin.meddraversion, pin.name
	FROM [dbo].[PRODUCT_PI_MN] as mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].[PRODUCT_INDICATION] pin ON pin.product_indications_PK = mn.product_indications_FK
	WHERE (mn.product_FK = @Product_FK OR @Product_FK IS NULL)

END
