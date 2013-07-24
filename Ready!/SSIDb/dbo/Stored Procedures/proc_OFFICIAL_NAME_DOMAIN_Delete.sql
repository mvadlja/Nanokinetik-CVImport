
-- Delete
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_DOMAIN_Delete]
	@on_domain_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[OFFICIAL_NAME_DOMAIN] WHERE [on_domain_PK] = @on_domain_PK
END
