
-- GetEntities
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_TYPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[official_name_type_PK], [type_name]
	FROM [dbo].[OFFICIAL_NAME_TYPE]
END
