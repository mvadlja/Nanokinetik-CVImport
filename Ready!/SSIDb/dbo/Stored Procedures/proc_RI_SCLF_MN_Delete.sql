
-- Delete
CREATE PROCEDURE [dbo].[proc_RI_SCLF_MN_Delete]
	@ri_sclf_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RI_SCLF_MN] WHERE [ri_sclf_mn_PK] = @ri_sclf_mn_PK
END
