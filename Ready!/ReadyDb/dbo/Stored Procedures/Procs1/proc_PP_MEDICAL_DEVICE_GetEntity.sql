-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PP_MEDICAL_DEVICE_GetEntity]
	@medicaldevice_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[medicaldevice_PK], [medicaldevicecode], [ev_code]
	FROM [dbo].[PP_MEDICAL_DEVICE]
	WHERE [medicaldevice_PK] = @medicaldevice_PK
END
