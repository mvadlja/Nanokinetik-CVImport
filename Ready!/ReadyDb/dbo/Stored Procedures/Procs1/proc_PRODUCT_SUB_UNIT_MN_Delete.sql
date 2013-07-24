-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_Delete]
	@product_submission_unit_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_SUB_UNIT_MN] WHERE [product_submission_unit_PK] = @product_submission_unit_PK
END
