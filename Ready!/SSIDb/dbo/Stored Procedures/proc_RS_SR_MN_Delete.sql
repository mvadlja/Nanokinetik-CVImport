
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_SR_MN_Delete]
	@rs_sr_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_SR_MN] WHERE [rs_sr_mn_PK] = @rs_sr_mn_PK
END
