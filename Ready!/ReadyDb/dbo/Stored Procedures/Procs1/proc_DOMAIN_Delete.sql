-- Delete
CREATE PROCEDURE  [dbo].[proc_DOMAIN_Delete]
	@domain_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[DOMAIN] WHERE [domain_PK] = @domain_PK
END
