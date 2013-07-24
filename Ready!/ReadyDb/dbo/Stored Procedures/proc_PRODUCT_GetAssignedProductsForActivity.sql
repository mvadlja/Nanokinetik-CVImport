-- GetAvailableProductsForActivity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetAssignedProductsForActivity]
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [dbo].[PRODUCT].[product_PK], [dbo].[PRODUCT].[newownerid], [dbo].[PRODUCT].[senderlocalcode], [dbo].[PRODUCT].[orphan_drug], [dbo].[PRODUCT].[intensive_monitoring], [dbo].[PRODUCT].[authorisation_procedure], [dbo].[PRODUCT].[comments], [dbo].[PRODUCT].[responsible_user_person_FK], [dbo].[PRODUCT].[psur], [dbo].[PRODUCT].[next_dlp], [dbo].[PRODUCT].[name], [dbo].[PRODUCT].[description], [dbo].[PRODUCT].[client_organization_FK], [dbo].[PRODUCT].[type_product_FK], [dbo].[PRODUCT].[product_number], [dbo].[PRODUCT].[product_ID],
	client_group_FK, region_FK, batch_size, pack_size, storage_conditions_FK
	FROM [dbo].[PRODUCT]
	WHERE product_PK IN
	(
		SELECT DISTINCT product_FK FROM [dbo].[ACTIVITY_PRODUCT_MN]
		WHERE activity_FK = @activity_FK
	)
	
END