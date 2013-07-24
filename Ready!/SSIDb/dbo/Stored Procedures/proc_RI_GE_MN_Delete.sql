
-- Delete
CREATE PROCEDURE [dbo].[proc_RI_GE_MN_Delete]
	@ri_ge_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RI_GE_MN] WHERE [ri_ge_mn_PK] = @ri_ge_mn_PK
END
