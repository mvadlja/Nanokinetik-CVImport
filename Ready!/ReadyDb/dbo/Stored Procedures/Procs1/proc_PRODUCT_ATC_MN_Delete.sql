-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ATC_MN_Delete]
	@product_atc_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_ATC_MN] WHERE [product_atc_mn_PK] = @product_atc_mn_PK
END
