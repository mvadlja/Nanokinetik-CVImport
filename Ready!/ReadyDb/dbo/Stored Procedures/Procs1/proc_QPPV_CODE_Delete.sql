-- Delete
CREATE PROCEDURE  [dbo].[proc_QPPV_CODE_Delete]
	@qppv_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[QPPV_CODE] WHERE [qppv_code_PK] = @qppv_code_PK
END
