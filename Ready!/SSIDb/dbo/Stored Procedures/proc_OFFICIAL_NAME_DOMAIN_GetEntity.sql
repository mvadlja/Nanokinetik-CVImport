
-- GetEntity
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_DOMAIN_GetEntity]
	@on_domain_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[on_domain_PK], [name_domain]
	FROM [dbo].[OFFICIAL_NAME_DOMAIN]
	WHERE [on_domain_PK] = @on_domain_PK
END
