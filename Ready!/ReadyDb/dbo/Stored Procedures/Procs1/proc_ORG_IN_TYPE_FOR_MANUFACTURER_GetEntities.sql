-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[org_in_type_for_manufacturer_ID], [organization_FK], [org_type_for_manu_FK], [product_FK], [substance_FK], 
	[dbo].SUBSTANCES.substance_name as SubstanceName,
	[dbo].[TYPE].name as ManufacturerName
	FROM [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER]
	LEFT JOIN [dbo].SUBSTANCES ON [dbo].SUBSTANCES.substance_PK = [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].substance_FK
	LEFT JOIN [dbo].[TYPE] ON [dbo].[TYPE].type_PK = [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].org_type_for_manu_FK
END
