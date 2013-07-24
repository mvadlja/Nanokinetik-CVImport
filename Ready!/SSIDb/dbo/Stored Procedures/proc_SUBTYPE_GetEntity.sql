
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SUBTYPE_GetEntity]
	@subtype_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[subtype_PK], [substance_class_subtype]
	FROM [dbo].[SUBTYPE]
	WHERE [subtype_PK] = @subtype_PK
END
