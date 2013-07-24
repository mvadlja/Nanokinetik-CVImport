
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_SC_MN_Delete]
	@rs_sc_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_SC_MN] WHERE [rs_sc_mn_PK] = @rs_sc_mn_PK
END
