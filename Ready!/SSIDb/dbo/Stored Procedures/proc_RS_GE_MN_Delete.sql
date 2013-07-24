
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_GE_MN_Delete]
	@rs_ge_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_GE_MN] WHERE [rs_ge_mn_PK] = @rs_ge_mn_PK
END
