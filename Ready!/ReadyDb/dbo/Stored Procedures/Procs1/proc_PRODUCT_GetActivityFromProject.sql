-- GetAvailableProductsForActivity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetActivityFromProject]
	@project int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT *
	FROM [dbo].ACTIVITY
	JOIN dbo.ACTIVITY_PROJECT_MN on activity.activity_PK = activity_FK
	where project_FK=@project
	
END
