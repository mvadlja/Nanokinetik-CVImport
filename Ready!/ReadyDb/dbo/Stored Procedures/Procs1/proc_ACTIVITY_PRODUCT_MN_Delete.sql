-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PRODUCT_MN_Delete]
	@activity_product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_PRODUCT_MN] WHERE [activity_product_PK] = @activity_product_PK
END
