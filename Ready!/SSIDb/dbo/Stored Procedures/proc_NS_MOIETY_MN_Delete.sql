
-- Delete
CREATE PROCEDURE [dbo].[proc_NS_MOIETY_MN_Delete]
	@ns_moiety_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[NS_MOIETY_MN] WHERE [ns_moiety_mn_PK] = @ns_moiety_mn_PK
END
