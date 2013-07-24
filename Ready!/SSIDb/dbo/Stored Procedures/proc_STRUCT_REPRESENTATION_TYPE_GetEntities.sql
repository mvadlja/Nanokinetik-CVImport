
-- GetEntities
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRESENTATION_TYPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[struct_repres_type_PK], [name]
	FROM [dbo].[STRUCT_REPRESENTATION_TYPE]
END
