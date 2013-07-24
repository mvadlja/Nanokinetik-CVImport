-- GetApplicantsByActivity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_ORGANIZATION_APP_MN_GetApplicantsByActivity]
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.activity_organization_applicant_PK, a.activity_PK, o.organization_PK, o.name_org
	FROM [dbo].[ACTIVITY_ORGANIZATION_APP_MN] mn
	LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = mn.activity_FK
	LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = mn.organization_FK
	WHERE (mn.activity_FK = @activity_FK OR @activity_FK IS NULL)

END
