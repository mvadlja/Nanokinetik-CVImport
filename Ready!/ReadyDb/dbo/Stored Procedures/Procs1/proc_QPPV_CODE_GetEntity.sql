-- GetEntity
CREATE PROCEDURE  [dbo].[proc_QPPV_CODE_GetEntity]
	@qppv_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[qppv_code_PK], [person_FK], [qppv_code]
	FROM [dbo].[QPPV_CODE]
	WHERE [qppv_code_PK] = @qppv_code_PK
END
