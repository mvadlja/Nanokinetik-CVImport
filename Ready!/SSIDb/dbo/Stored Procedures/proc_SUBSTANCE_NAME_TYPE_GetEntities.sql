
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_TYPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_name_type_PK], [name]
	FROM [dbo].[SUBSTANCE_NAME_TYPE]
END
