-- GetProductsByActivity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PRODUCT_MN_GetProductsByActivity]
	@activity_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.activity_product_PK, p.product_PK, p.name, p.ProductName, a.activity_PK,p.product_number
	FROM [dbo].[ACTIVITY_PRODUCT_MN] mn
	LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = mn.activity_FK
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	WHERE (mn.activity_FK = @activity_PK OR @activity_PK IS NULL)

END
