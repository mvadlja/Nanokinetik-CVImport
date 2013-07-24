
-- Delete
CREATE PROCEDURE [dbo].[proc_NS_PROPERTY_MN_Delete]
	@ns_property_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[NS_PROPERTY_MN] WHERE [ns_property_mn_PK] = @ns_property_mn_PK
END
