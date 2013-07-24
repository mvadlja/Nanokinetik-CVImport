
-- GetEntities
CREATE PROCEDURE [dbo].[proc_NS_PROPERTY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ns_property_mn_PK], [ns_FK], [property_FK]
	FROM [dbo].[NS_PROPERTY_MN]
END
