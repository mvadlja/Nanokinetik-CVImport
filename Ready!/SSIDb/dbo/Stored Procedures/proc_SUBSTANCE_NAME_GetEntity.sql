
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_GetEntity]
	@substance_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_name_PK], [subst_name], [subst_name_type_FK], [language_FK]
	FROM [dbo].[SUBSTANCE_NAME]
	WHERE [substance_name_PK] = @substance_name_PK
END
