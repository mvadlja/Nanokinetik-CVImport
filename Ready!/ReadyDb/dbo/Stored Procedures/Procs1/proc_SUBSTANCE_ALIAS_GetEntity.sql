-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_GetEntity]
	@substance_alias_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_alias_PK], [sourcecode], [resolutionmode], [aliasname], [substance_aliastranslations_FK]
	FROM [dbo].[SUBSTANCE_ALIAS]
	WHERE [substance_alias_PK] = @substance_alias_PK
END
