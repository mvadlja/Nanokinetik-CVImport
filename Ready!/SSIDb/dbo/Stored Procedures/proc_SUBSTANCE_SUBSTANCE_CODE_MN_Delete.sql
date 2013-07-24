
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_CODE_MN_Delete]
	@substance_substance_code_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN] WHERE [substance_substance_code_mn_PK] = @substance_substance_code_mn_PK
END
