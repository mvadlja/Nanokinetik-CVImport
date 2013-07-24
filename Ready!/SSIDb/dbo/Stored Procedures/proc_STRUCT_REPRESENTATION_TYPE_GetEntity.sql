
-- GetEntity
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRESENTATION_TYPE_GetEntity]
	@struct_repres_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[struct_repres_type_PK], [name]
	FROM [dbo].[STRUCT_REPRESENTATION_TYPE]
	WHERE [struct_repres_type_PK] = @struct_repres_type_PK
END
