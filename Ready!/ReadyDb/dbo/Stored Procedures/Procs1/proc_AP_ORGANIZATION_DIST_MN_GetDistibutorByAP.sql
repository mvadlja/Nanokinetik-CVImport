-- GetDistibutorByAP
CREATE PROCEDURE  [dbo].[proc_AP_ORGANIZATION_DIST_MN_GetDistibutorByAP]
	@ap_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[AP_ORGANIZATION_DIST_MN].ap_organizatation_dist_mn_PK, o.name_org, o.organization_PK
	FROM [dbo].[AP_ORGANIZATION_DIST_MN]
	LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = [dbo].[AP_ORGANIZATION_DIST_MN].[organization_FK]
	WHERE ([dbo].[AP_ORGANIZATION_DIST_MN].[ap_FK] = @ap_FK OR @ap_FK IS NULL)

END
