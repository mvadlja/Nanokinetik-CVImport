
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetAssignedAgenciesForSubmissionUnit]
	@submissionUnitPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT o.*
	FROM [dbo].[ORGANIZATION_IN_ROLE]
	LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = [dbo].[ORGANIZATION_IN_ROLE].organization_FK
	LEFT JOIN [dbo].[ORGANIZATION_ROLE] r ON r.role_org_PK = [dbo].[ORGANIZATION_IN_ROLE].[role_org_FK]
	WHERE LOWER(r.role_name) = 'agency' AND o.[organization_PK] IN
	(
		select agency_FK from dbo.SU_AGENCY_MN suAgencyMn
		where submission_unit_FK = @submissionUnitPk AND @submissionUnitPk IS NOT NULL
	)
	
END