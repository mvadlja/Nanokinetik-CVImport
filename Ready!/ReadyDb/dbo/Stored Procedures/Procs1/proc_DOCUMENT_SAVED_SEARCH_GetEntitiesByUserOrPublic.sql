-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_SAVED_SEARCH_GetEntitiesByUserOrPublic]
	@user_fk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	*
	FROM [dbo].DOCUMENT_SAVED_SEARCH
	where [user_FK1]=@user_fk or [ispublic]=1
END
