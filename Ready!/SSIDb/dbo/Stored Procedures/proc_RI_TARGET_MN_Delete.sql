
-- Delete
CREATE PROCEDURE [dbo].[proc_RI_TARGET_MN_Delete]
	@ri_target_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RI_TARGET_MN] WHERE [ri_target_mn_PK] = @ri_target_mn_PK
END
