-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SAVED_SEARCH_GetEntitiesByUserOrPublic]
	@user_fk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	*
	FROM [dbo].PRODUCT_SAVED_SEARCH
	where [user_FK]=@user_fk or [ispublic]=1
END
