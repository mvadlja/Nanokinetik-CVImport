-- GetProductsByActivity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PRODUCT_MN_GetProductsByActivityList]
	@activity_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.activity_product_PK, mn.product_FK, mn.activity_FK
	FROM [dbo].[ACTIVITY_PRODUCT_MN] mn
	WHERE (mn.activity_FK = @activity_PK OR @activity_PK IS NULL)

END
