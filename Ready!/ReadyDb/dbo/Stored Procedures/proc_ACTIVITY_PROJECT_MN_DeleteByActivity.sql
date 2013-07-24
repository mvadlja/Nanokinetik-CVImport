-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PROJECT_MN_DeleteByActivity]
	@ActivityPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_PROJECT_MN] WHERE activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL
END