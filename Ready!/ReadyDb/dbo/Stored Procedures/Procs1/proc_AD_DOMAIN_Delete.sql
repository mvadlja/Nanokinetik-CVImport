-- Delete
CREATE PROCEDURE  [dbo].[proc_AD_DOMAIN_Delete]
	@ad_domain_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AD_DOMAIN] WHERE [ad_domain_PK] = @ad_domain_PK
END
