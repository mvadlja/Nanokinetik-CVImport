-- GetEntity
CREATE PROCEDURE  [dbo].[proc_LANGUAGE_CODE_GetEntity]
	@languagecode_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[languagecode_PK], [name], [code]
	FROM [dbo].[LANGUAGE_CODE]
	WHERE [languagecode_PK] = @languagecode_PK
END
