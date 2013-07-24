-- GetEntities
CREATE PROCEDURE  [dbo].[proc_MEDDRA_AP_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[meddra_ap_mn_PK], [ap_FK], [meddra_FK]
	FROM [dbo].[MEDDRA_AP_MN]
END
