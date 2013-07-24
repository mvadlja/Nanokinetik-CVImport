
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBTYPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[subtype_PK], [substance_class_subtype]
	FROM [dbo].[SUBTYPE]
END
