-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntitiesByUserOrPublic]
	@user_fk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	*
	FROM [dbo].PHARMACEUTICAL_PRODUCT_SAVED_SEARCH
	where [user_FK]=@user_fk or [ispublic]=1
END
