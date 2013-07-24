
create PROCEDURE  [dbo].[proc_PRODUCT_GetProductsByActivity]
	@activity_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM [dbo].[ACTIVITY_PRODUCT_MN] mn
	LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = mn.activity_FK
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	WHERE (mn.activity_FK = @activity_PK OR @activity_PK IS NULL)

END