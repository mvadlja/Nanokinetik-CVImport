-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetEntitiesByPharmaceuticalProduct]
	@PharmaceuticalProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_PK], [newownerid], [senderlocalcode], [orphan_drug], [intensive_monitoring], [authorisation_procedure], [comments], [responsible_user_person_FK], [psur], [next_dlp], [name], [description], [client_organization_FK], [type_product_FK], [product_number], [product_ID], [mrp_dcp], [eu_number],
	client_group_FK, region_FK, batch_size, pack_size, storage_conditions_FK
	FROM [dbo].PRODUCT_PP_MN
	JOIN [dbo].[PRODUCT] ON [dbo].[PRODUCT].product_PK = [dbo].PRODUCT_PP_MN.product_FK
	WHERE [dbo].PRODUCT_PP_MN.pp_FK = @PharmaceuticalProductPk AND @PharmaceuticalProductPk IS NOT NULL
END