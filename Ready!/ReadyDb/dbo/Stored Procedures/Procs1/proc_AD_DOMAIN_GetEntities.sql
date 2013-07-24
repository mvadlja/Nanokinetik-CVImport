-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AD_DOMAIN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ad_domain_PK], [domain_alias], [domain_connection_string]
	FROM [dbo].[AD_DOMAIN]
END
