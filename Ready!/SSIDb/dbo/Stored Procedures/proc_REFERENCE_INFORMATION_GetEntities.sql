
-- GetEntities
CREATE PROCEDURE [dbo].[proc_REFERENCE_INFORMATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reference_info_PK], [comment]
	FROM [dbo].[REFERENCE_INFORMATION]
END
