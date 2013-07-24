-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_pi_mn_PK], [product_indications_FK], [product_FK]
	FROM [dbo].[PRODUCT_PI_MN]
END
