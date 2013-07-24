-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_submission_unit_PK], [product_FK], [submission_unit_FK]
	FROM [dbo].[PRODUCT_SUB_UNIT_MN]
END
