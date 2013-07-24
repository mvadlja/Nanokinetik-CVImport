
-- Delete
CREATE PROCEDURE [dbo].[proc_ON_DOMAIN_ON_MN_Delete]
	@on_domain_on_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ON_DOMAIN_ON_MN] WHERE [on_domain_on_mn_PK] = @on_domain_on_mn_PK
END
