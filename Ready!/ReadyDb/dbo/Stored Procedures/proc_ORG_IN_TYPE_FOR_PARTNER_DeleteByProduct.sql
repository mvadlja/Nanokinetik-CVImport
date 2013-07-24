-- Delete
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_PARTNER_DeleteByProduct]
	@ProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORG_IN_TYPE_FOR_PARTNER] WHERE [product_FK] = @ProductPk AND @ProductPk IS NOT NULL
END