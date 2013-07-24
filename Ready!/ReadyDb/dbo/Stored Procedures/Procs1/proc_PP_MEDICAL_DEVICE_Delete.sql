-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_MEDICAL_DEVICE_Delete]
	@medicaldevice_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_MEDICAL_DEVICE] WHERE [medicaldevice_PK] = @medicaldevice_PK
END
