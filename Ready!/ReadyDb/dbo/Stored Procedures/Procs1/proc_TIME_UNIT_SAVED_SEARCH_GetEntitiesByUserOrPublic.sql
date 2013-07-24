-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_SAVED_SEARCH_GetEntitiesByUserOrPublic]
	@user_fk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	*
	FROM [dbo].TIME_UNIT_SAVED_SEARCH
	where [user_FK1]=@user_fk or [ispublic]=1
END
