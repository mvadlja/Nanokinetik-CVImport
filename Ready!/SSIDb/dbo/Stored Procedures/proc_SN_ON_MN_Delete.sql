
-- Delete
CREATE PROCEDURE [dbo].[proc_SN_ON_MN_Delete]
	@sn_on_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SN_ON_MN] WHERE [sn_on_mn_PK] = @sn_on_mn_PK
END
