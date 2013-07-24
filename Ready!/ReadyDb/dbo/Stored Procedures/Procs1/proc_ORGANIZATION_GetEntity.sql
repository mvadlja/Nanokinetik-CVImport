-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetEntity]
	@organization_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	*
	FROM [dbo].[ORGANIZATION]
	WHERE [organization_PK] = @organization_PK
END
