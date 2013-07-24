-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ATC_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_atc_mn_PK], [product_FK], [atc_FK]
	FROM [dbo].[PRODUCT_ATC_MN]
END
