-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_DeleteByProductPK]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_PP_MN] WHERE [product_FK] = @Product_FK
END
