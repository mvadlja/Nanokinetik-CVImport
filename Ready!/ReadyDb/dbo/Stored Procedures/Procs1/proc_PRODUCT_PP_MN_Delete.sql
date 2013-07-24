-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_Delete]
	@product_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_PP_MN] WHERE [product_mn_PK] = @product_mn_PK
END
