-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_alias_PK], [sourcecode], [resolutionmode], [aliasname], [substance_aliastranslations_FK]
	FROM [dbo].[SUBSTANCE_ALIAS]
END
