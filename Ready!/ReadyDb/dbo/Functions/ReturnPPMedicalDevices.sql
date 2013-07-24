-- =============================================
-- Author:		<Denis,Petek>
-- Create date: <08.03.2012.>
-- =============================================
CREATE FUNCTION [dbo].[ReturnPPMedicalDevices]

(
	@pp_pk INT
)

RETURNS nvarchar(4000)
AS
BEGIN
	-- Declare the return variable here
	declare @return_value nvarchar(max)

	SELECT @return_value = COALESCE(@return_value + ', ', '') +
			isnull(rtrim(ltrim( medical_device.medicaldevicecode )), '')
	FROM dbo.PP_MEDICAL_DEVICE medical_device
	LEFT JOIN dbo.PP_MD_MN md_mn
	ON medical_device.medicaldevice_PK = md_mn.pp_medical_device_FK
	where md_mn.pharmaceutical_product_FK = @pp_pk

	ORDER BY medical_device.medicaldevicecode
	
	return @return_value;

END
