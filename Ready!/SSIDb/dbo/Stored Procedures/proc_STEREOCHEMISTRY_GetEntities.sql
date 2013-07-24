
-- GetEntities
CREATE PROCEDURE [dbo].[proc_STEREOCHEMISTRY_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[stereochemistry_PK], [name]
	FROM [dbo].[STEREOCHEMISTRY]
END
