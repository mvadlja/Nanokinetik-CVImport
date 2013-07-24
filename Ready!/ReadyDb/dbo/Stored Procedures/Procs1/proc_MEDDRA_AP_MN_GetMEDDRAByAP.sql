-- GetCountriesByActivity
CREATE PROCEDURE  [dbo].[proc_MEDDRA_AP_MN_GetMEDDRAByAP]
	@ap_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.meddra_ap_mn_PK, ap.ap_PK, m.*,
		(SELECT [dbo].[TYPE].name FROM [dbo].[TYPE] WHERE [dbo].[TYPE].type_PK=m.version_type_FK) AS version_name,
		(SELECT [dbo].[TYPE].name FROM [dbo].[TYPE] WHERE [dbo].[TYPE].type_PK=m.level_type_FK) AS level_name
	FROM [dbo].[MEDDRA_AP_MN] mn
	LEFT JOIN [dbo].[AUTHORISED_PRODUCT] ap ON ap.ap_PK = mn.ap_FK
	LEFT JOIN [dbo].[MEDDRA] m ON m.meddra_pk = mn.meddra_FK
	WHERE (mn.ap_FK = @ap_FK)

END
