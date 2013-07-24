-- GetOrganisationInRolesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_PP_MD_MN_GetMedDevicesByPPPK]
	@pharmaceutical_product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PP_MD_MN].[pp_md_mn_PK], [dbo].[PP_MD_MN].[pharmaceutical_product_FK], [dbo].[PP_MD_MN].[pp_medical_device_FK]
	FROM [dbo].[PP_MD_MN]
	WHERE ([dbo].[PP_MD_MN].[pharmaceutical_product_FK] = @pharmaceutical_product_FK OR @pharmaceutical_product_FK IS NULL)

END
