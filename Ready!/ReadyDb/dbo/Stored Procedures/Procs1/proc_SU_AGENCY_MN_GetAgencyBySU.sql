-- GetDistibutorByAP
CREATE PROCEDURE  [dbo].[proc_SU_AGENCY_MN_GetAgencyBySU]
	@su_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[SU_AGENCY_MN].su_agency_mn_PK, [dbo].[SU_AGENCY_MN].submission_unit_FK, o.name_org, o.organization_PK
	FROM [dbo].[SU_AGENCY_MN]
	LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = [dbo].[SU_AGENCY_MN].[agency_FK]
	WHERE ([dbo].[SU_AGENCY_MN].[submission_unit_FK] = @su_FK OR @su_FK IS NULL)

END
