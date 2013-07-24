
-- GetEntity
CREATE PROCEDURE [dbo].[proc_NS_MOIETY_MN_GetEntity]
	@ns_moiety_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ns_moiety_mn_PK], [moiety_FK], [ns_FK]
	FROM [dbo].[NS_MOIETY_MN]
	WHERE [ns_moiety_mn_PK] = @ns_moiety_mn_PK
END
