-- Delete
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_MANUFACTURER_DeleteByProduct]
	@ProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER] WHERE [product_FK] = @ProductPk AND @ProductPk IS NOT NULL
END