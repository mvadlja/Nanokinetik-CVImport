-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntitiesByProduct]
	@ProductPk INT = NULL
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[org_in_type_for_manufacturer_ID], [organization_FK], [org_type_for_manu_FK], [product_FK], [substance_FK], 
	CASE 
	WHEN [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[organization_FK] IS NOT NULL 
	THEN
	(	SELECT [dbo].ORGANIZATION.name_org
		FROM [dbo].ORGANIZATION
		WHERE  [dbo].ORGANIZATION.organization_PK = [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].[organization_FK])
	ELSE NULL
	END as ManufacturerName,
	CASE 
	WHEN [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].org_type_for_manu_FK IS NOT NULL 
	THEN
	(	SELECT [dbo].[TYPE].name
		FROM [dbo].[TYPE]
		WHERE  [dbo].[TYPE].type_PK = [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].org_type_for_manu_FK)
	ELSE NULL
	END as ManufacturerTypeName,
	CASE 
	WHEN [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].substance_FK IS NOT NULL 
	THEN
	(	SELECT [dbo].SUBSTANCES.substance_name
		FROM [dbo].SUBSTANCES
		WHERE  [dbo].SUBSTANCES.substance_PK = [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER].substance_FK)
	ELSE NULL
	END as SubstanceName
	FROM [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER]
	where [product_FK] = @ProductPk AND @ProductPk IS NOT NULL
END