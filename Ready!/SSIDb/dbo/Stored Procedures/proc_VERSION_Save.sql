
-- Save
CREATE PROCEDURE [dbo].[proc_VERSION_Save]
	@version_PK int = NULL,
	@version_number int = NULL,
	@effectve_date varchar(10) = NULL,
	@change_made varchar(4000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[VERSION]
	SET
	[version_number] = @version_number,
	[effectve_date] = @effectve_date,
	[change_made] = @change_made
	WHERE [version_PK] = @version_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[VERSION]
		([version_number], [effectve_date], [change_made])
		VALUES
		(@version_number, @effectve_date, @change_made)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
