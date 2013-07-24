-- Delete
CREATE PROCEDURE  [dbo].[proc_AP_ORGANIZATION_DIST_MN_Delete]
	@ap_organizatation_dist_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AP_ORGANIZATION_DIST_MN] WHERE [ap_organizatation_dist_mn_PK] = @ap_organizatation_dist_mn_PK
END
