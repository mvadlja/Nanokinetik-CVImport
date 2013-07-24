-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_Delete]
	@product_pi_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_PI_MN] WHERE [product_pi_mn_PK] = @product_pi_mn_PK
END
