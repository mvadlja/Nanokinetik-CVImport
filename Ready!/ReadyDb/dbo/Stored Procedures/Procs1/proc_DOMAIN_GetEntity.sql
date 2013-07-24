-- GetEntity
CREATE PROCEDURE  [dbo].[proc_DOMAIN_GetEntity]
	@domain_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[domain_PK], [name]
	FROM [dbo].[DOMAIN]
	WHERE [domain_PK] = @domain_PK
END
