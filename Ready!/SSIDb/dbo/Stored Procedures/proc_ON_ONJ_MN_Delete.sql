
-- Delete
CREATE PROCEDURE [dbo].[proc_ON_ONJ_MN_Delete]
	@on_onj_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ON_ONJ_MN] WHERE [on_onj_mn_PK] = @on_onj_mn_PK
END
