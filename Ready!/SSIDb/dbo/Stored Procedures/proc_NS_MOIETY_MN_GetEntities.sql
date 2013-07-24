
-- GetEntities
CREATE PROCEDURE [dbo].[proc_NS_MOIETY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ns_moiety_mn_PK], [moiety_FK], [ns_FK]
	FROM [dbo].[NS_MOIETY_MN]
END
