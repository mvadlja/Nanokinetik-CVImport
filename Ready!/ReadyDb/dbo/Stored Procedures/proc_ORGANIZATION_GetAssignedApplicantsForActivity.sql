
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetAssignedApplicantsForActivity]
	@ActivityPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[ORGANIZATION]
	WHERE [dbo].[ORGANIZATION].[organization_PK] IN
	(
		select organization_FK from dbo.ACTIVITY_ORGANIZATION_APP_MN
		where activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL
	)
	
END