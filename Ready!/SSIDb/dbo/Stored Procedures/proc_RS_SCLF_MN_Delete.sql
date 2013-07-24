
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_SCLF_MN_Delete]
	@rs_sclf_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_SCLF_MN] WHERE [rs_sclf_mn_PK] = @rs_sclf_mn_PK
END
