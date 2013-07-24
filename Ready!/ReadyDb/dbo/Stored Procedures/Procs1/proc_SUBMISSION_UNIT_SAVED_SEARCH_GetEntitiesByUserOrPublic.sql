-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_SAVED_SEARCH_GetEntitiesByUserOrPublic]
	@user_fk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	*
	FROM [dbo].SUBMISSION_UNIT_SAVED_SEARCH
	where [user_FK]=@user_fk or [ispublic]=1
END
