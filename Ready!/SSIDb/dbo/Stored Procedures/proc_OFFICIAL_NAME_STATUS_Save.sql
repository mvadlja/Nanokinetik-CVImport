
-- Save
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_STATUS_Save]
	@official_name_status_PK int = NULL,
	@status_name nchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[OFFICIAL_NAME_STATUS]
	SET
	[status_name] = @status_name
	WHERE [official_name_status_PK] = @official_name_status_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[OFFICIAL_NAME_STATUS]
		([status_name])
		VALUES
		(@status_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
