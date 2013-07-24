-- Delete
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_MANUFACTURER_Delete]
	@org_in_type_for_manufacturer_ID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER] WHERE [org_in_type_for_manufacturer_ID] = @org_in_type_for_manufacturer_ID
END
