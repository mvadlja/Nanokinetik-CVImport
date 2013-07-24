-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AP_ORGANIZATION_DIST_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ap_organizatation_dist_mn_PK], [organization_FK], [ap_FK]
	FROM [dbo].[AP_ORGANIZATION_DIST_MN]
END
