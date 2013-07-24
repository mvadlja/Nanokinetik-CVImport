
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetAvailableApplicantsForActivity]
	@ActivityPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT o.*
	FROM [dbo].[ORGANIZATION] o
	JOIN dbo.ORGANIZATION_IN_ROLE orMn ON orMn.organization_FK = o.organization_PK
	JOIN dbo.ORGANIZATION_ROLE r ON r.role_org_PK = orMn.role_org_FK
	WHERE o.[organization_PK] NOT IN
	(
		select organization_FK from dbo.ACTIVITY_ORGANIZATION_APP_MN
		where activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL
	)
	AND r.role_name = 'Applicant'
	
END