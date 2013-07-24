-- GetEntities
create PROCEDURE  [dbo].[proc_PP_MEDICAL_DEVICE_GetEntitiesByPharmaceuticalProduct]
	@PharmaceuticalProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[dbo].PP_MEDICAL_DEVICE.[medicaldevice_PK], [dbo].PP_MEDICAL_DEVICE.[medicaldevicecode], [dbo].PP_MEDICAL_DEVICE.[ev_code]
	FROM [dbo].PP_MD_MN
	JOIN [dbo].PP_MEDICAL_DEVICE ON [dbo].PP_MEDICAL_DEVICE.medicaldevice_PK = [dbo].PP_MD_MN.pp_medical_device_FK
	WHERE [dbo].PP_MD_MN.pharmaceutical_product_FK = @PharmaceuticalProductPk AND @PharmaceuticalProductPk IS NOT NULL

END