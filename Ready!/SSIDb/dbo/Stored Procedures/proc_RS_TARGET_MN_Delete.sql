
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_TARGET_MN_Delete]
	@rs_target_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_TARGET_MN] WHERE [rs_target_mn_PK] = @rs_target_mn_PK
END
