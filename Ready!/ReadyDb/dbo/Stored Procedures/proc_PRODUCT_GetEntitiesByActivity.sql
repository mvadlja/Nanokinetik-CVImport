-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetEntitiesByActivity]
	@ActivityPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT p.*
	FROM [dbo].ACTIVITY_PRODUCT_MN paMn
	JOIN [dbo].[PRODUCT] p ON p.product_PK = paMn.product_FK
	WHERE paMn.activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL
END