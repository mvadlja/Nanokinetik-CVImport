-- GetEntities
CREATE PROCEDURE  [dbo].[proc_INTERNATIONAL_CODE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[international_code_PK], [sourcecode], [resolutionmode_sources], [referencetext], [resolutionmode_substance]
	FROM [dbo].[INTERNATIONAL_CODE]
END
