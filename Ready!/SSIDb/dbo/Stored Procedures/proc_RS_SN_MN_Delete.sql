
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_SN_MN_Delete]
	@rs_sn_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_SN_MN] WHERE [rs_sn_mn_PK] = @rs_sn_mn_PK
END
