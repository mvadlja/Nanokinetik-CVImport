
-- GetEntities
CREATE PROCEDURE [dbo].[proc_VERSION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[version_PK], [version_number], [effectve_date], [change_made]
	FROM [dbo].[VERSION]
END
