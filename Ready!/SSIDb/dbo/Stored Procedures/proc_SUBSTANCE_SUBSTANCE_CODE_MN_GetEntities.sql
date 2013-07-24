
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_substance_code_mn_PK], [substance_FK], [substance_code_FK]
	FROM [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN]
END
