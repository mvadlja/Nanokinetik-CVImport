
-- GetEntity
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_TYPE_GetEntity]
	@official_name_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[official_name_type_PK], [type_name]
	FROM [dbo].[OFFICIAL_NAME_TYPE]
	WHERE [official_name_type_PK] = @official_name_type_PK
END
