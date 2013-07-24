-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_GetEntity]
	@product_pi_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_pi_mn_PK], [product_indications_FK], [product_FK]
	FROM [dbo].[PRODUCT_PI_MN]
	WHERE [product_pi_mn_PK] = @product_pi_mn_PK
END
