-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PP_MD_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pp_md_mn_PK], [pp_medical_device_FK], [pharmaceutical_product_FK]
	FROM [dbo].[PP_MD_MN]
END
