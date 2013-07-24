-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AD_DOMAIN_GetEntity]
	@ad_domain_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ad_domain_PK], [domain_alias], [domain_connection_string]
	FROM [dbo].[AD_DOMAIN]
	WHERE [ad_domain_PK] = @ad_domain_PK
END
