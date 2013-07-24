-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TASK_SAVED_SEARCH_GetEntitiesByUserOrPublic]
	@user_fk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	*
	FROM [dbo].TASK_SAVED_SEARCH
	where [user_FK1]=@user_fk or [ispublic]=1
END
