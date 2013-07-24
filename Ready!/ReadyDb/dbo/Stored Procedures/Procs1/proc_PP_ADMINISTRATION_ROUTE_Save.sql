-- Save
CREATE PROCEDURE  [dbo].[proc_PP_ADMINISTRATION_ROUTE_Save]
	@adminroute_PK int = NULL,
	@adminroutecode nvarchar(60) = NULL,
	@resolutionmode int = NULL,
	@ev_code nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_ADMINISTRATION_ROUTE]
	SET
	[adminroutecode] = @adminroutecode,
	[resolutionmode] = @resolutionmode,
	[ev_code] = @ev_code
	WHERE [adminroute_PK] = @adminroute_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_ADMINISTRATION_ROUTE]
		([adminroutecode], [resolutionmode], [ev_code])
		VALUES
		(@adminroutecode, @resolutionmode, @ev_code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
