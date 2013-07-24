-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PP_MEDICAL_DEVICE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[medicaldevice_PK], [medicaldevicecode], [ev_code]
	FROM [dbo].[PP_MEDICAL_DEVICE]
END
