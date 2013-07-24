-- Save
CREATE PROCEDURE  [dbo].[proc_PP_MEDICAL_DEVICE_Save]
	@medicaldevice_PK int = NULL,
	@medicaldevicecode nvarchar(60) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_MEDICAL_DEVICE]
	SET
	[medicaldevicecode] = @medicaldevicecode
	WHERE [medicaldevice_PK] = @medicaldevice_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_MEDICAL_DEVICE]
		([medicaldevicecode])
		VALUES
		(@medicaldevicecode)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
