-- GetAvailableProductsForActivity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetActivityFromProduct]
	@product int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT *,dbo.ReturnActivityForProject([dbo].ACTIVITY.activity_PK)as products
	FROM [dbo].ACTIVITY
	JOIN dbo.ACTIVITY_PRODUCT_MN on activity.activity_PK = activity_product_MN.activity_FK
	where product_FK=@product
	
END
