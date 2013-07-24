-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ATC_MN_DeleteByProductPK]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_ATC_MN] WHERE [product_FK] = @Product_FK
END
