-- GetEntities
CREATE PROCEDURE  [dbo].[proc_LANGUAGE_CODE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[languagecode_PK], [name], [code]
	FROM [dbo].[LANGUAGE_CODE]
END
