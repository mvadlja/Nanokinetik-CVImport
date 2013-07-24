-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PRODUCT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_product_PK], [activity_FK], [product_FK]
	FROM [dbo].[ACTIVITY_PRODUCT_MN]
END
