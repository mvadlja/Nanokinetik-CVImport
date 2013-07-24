
-- GetEntity
CREATE PROCEDURE [dbo].[proc_NS_PROPERTY_MN_GetEntity]
	@ns_property_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ns_property_mn_PK], [ns_FK], [property_FK]
	FROM [dbo].[NS_PROPERTY_MN]
	WHERE [ns_property_mn_PK] = @ns_property_mn_PK
END
