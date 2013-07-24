-- GetAvailableProductsForActivity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetAvailableProductsForActivity]
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT product_PK, newownerid, senderlocalcode, orphan_drug, intensive_monitoring, authorisation_procedure, comments, responsible_user_person_FK, psur, next_dlp, name, [description], client_organization_FK, type_product_FK, product_number, product_ID, mrp_dcp, eu_number, ProductName, Countries, ActiveSubstances, DrugAtcs,
	client_group_FK, region_FK, batch_size, pack_size, storage_conditions_FK
	FROM [dbo].[PRODUCT]
	WHERE product_PK NOT IN
	(
		SELECT DISTINCT product_FK FROM [dbo].[ACTIVITY_PRODUCT_MN]
		WHERE activity_FK = @activity_FK
	)
	
END
