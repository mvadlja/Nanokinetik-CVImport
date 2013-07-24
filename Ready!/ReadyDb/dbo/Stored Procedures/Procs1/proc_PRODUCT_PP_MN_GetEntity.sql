-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_GetEntity]
	@product_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_mn_PK], [product_FK], [pp_FK]
	FROM [dbo].[PRODUCT_PP_MN]
	WHERE [product_mn_PK] = @product_mn_PK
END
