
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_NAME_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_substance_name_mn_PK], [substance_FK], [substance_name_FK]
	FROM [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN]
END
