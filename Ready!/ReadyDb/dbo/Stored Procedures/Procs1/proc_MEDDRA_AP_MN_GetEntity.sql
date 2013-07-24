-- GetEntity
CREATE PROCEDURE  [dbo].[proc_MEDDRA_AP_MN_GetEntity]
	@meddra_ap_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[meddra_ap_mn_PK], [ap_FK], [meddra_FK]
	FROM [dbo].[MEDDRA_AP_MN]
	WHERE [meddra_ap_mn_PK] = @meddra_ap_mn_PK
END
