
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SN_ON_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sn_on_mn_PK], [official_name_FK], [substance_name_FK]
	FROM [dbo].[SN_ON_MN]
END
