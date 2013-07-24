-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_PARTNER_GetEntity]
	@org_in_type_for_partner_ID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

		SELECT
	[org_in_type_for_partner_ID], [organization_FK], [org_type_for_partner_FK], [product_FK], 
	CASE 
	WHEN [dbo].[ORG_IN_TYPE_FOR_PARTNER].[organization_FK] IS NOT NULL 
	THEN
	(	SELECT [dbo].ORGANIZATION.name_org
		FROM [dbo].ORGANIZATION
		WHERE  [dbo].ORGANIZATION.organization_PK = [dbo].[ORG_IN_TYPE_FOR_PARTNER].[organization_FK])
	ELSE NULL
	END as PartnerName,
	CASE 
	WHEN [dbo].[ORG_IN_TYPE_FOR_PARTNER].org_type_for_partner_FK IS NOT NULL 
	THEN
	(	SELECT [dbo].[TYPE].name
		FROM [dbo].[TYPE]
		WHERE  [dbo].[TYPE].type_PK = [dbo].[ORG_IN_TYPE_FOR_PARTNER].org_type_for_partner_FK)
	ELSE NULL
	END as PartnerTypeName
	FROM [dbo].[ORG_IN_TYPE_FOR_PARTNER]
	WHERE [org_in_type_for_partner_ID] = @org_in_type_for_partner_ID
END
