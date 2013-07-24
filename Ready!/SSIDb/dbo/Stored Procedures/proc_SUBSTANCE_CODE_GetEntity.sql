
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CODE_GetEntity]
	@substance_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_code_PK], [code], [code_system_FK], [code_system_id_FK], [code_system_status_FK], [code_system_changedate], [comment]
	FROM [dbo].[SUBSTANCE_CODE]
	WHERE [substance_code_PK] = @substance_code_PK
END
