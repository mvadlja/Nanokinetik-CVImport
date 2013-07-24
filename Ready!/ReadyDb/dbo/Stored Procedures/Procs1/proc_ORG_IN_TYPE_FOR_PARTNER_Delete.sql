-- Delete
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_PARTNER_Delete]
	@org_in_type_for_partner_ID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORG_IN_TYPE_FOR_PARTNER] WHERE [org_in_type_for_partner_ID] = @org_in_type_for_partner_ID
END
