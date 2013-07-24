-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PRODUCT_MN_GetEntity]
	@activity_product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_product_PK], [activity_FK], [product_FK]
	FROM [dbo].[ACTIVITY_PRODUCT_MN]
	WHERE [activity_product_PK] = @activity_product_PK
END
