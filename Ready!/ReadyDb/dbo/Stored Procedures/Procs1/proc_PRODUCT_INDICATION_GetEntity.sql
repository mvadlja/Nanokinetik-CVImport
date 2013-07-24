-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_INDICATION_GetEntity]
	@product_indications_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_indications_PK], [meddraversion], [meddralevel], [meddracode], [name]
	FROM [dbo].[PRODUCT_INDICATION]
	WHERE [product_indications_PK] = @product_indications_PK
END
