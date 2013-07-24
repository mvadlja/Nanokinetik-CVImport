-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_GetEntity]
	@product_submission_unit_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_submission_unit_PK], [product_FK], [submission_unit_FK]
	FROM [dbo].[PRODUCT_SUB_UNIT_MN]
	WHERE [product_submission_unit_PK] = @product_submission_unit_PK
END
