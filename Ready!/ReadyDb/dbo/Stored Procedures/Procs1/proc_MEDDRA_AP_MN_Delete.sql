-- Delete
CREATE PROCEDURE  [dbo].[proc_MEDDRA_AP_MN_Delete]
	@meddra_ap_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MEDDRA_AP_MN] WHERE [meddra_ap_mn_PK] = @meddra_ap_mn_PK
END
