-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AP_ORGANIZATION_DIST_MN_GetEntity]
	@ap_organizatation_dist_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ap_organizatation_dist_mn_PK], [organization_FK], [ap_FK]
	FROM [dbo].[AP_ORGANIZATION_DIST_MN]
	WHERE [ap_organizatation_dist_mn_PK] = @ap_organizatation_dist_mn_PK
END
