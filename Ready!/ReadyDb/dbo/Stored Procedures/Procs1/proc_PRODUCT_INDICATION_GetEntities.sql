-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_INDICATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_indications_PK], [meddraversion], [meddralevel], [meddracode], [name]
	FROM [dbo].[PRODUCT_INDICATION]
END
