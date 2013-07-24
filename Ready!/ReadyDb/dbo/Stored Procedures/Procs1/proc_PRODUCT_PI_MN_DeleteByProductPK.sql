-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_DeleteByProductPK]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_PI_MN] WHERE [product_FK] = @Product_FK
END
