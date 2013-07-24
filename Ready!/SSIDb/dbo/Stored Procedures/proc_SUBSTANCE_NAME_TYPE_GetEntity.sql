
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_TYPE_GetEntity]
	@substance_name_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_name_type_PK], [name]
	FROM [dbo].[SUBSTANCE_NAME_TYPE]
	WHERE [substance_name_type_PK] = @substance_name_type_PK
END
