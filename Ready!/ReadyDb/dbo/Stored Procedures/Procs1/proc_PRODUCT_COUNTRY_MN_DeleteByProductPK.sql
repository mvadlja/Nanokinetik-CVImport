-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_COUNTRY_MN_DeleteByProductPK]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_COUNTRY_MN] WHERE [product_FK] = @Product_FK
END
