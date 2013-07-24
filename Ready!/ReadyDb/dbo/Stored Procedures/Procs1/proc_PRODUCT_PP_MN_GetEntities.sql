-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_mn_PK], [product_FK], [pp_FK]
	FROM [dbo].[PRODUCT_PP_MN]
END
