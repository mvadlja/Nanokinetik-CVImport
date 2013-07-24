-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetEntity]
	@product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_PK], [newownerid], [senderlocalcode], [orphan_drug], [intensive_monitoring], [authorisation_procedure], [comments], [responsible_user_person_FK], [psur], [next_dlp], [name], [description], [client_organization_FK], [type_product_FK], [product_number], [product_ID], [mrp_dcp], [eu_number],
	client_group_FK, region_FK, batch_size, pack_size, storage_conditions_FK
	FROM [dbo].[PRODUCT]
	WHERE [product_PK] = @product_PK
END
