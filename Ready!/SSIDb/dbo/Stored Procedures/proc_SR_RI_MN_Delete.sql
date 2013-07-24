
-- Delete
CREATE PROCEDURE [dbo].[proc_SR_RI_MN_Delete]
	@sr_ri_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SR_RI_MN] WHERE [sr_ri_mn_PK] = @sr_ri_mn_PK
END
