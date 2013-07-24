-- Save
CREATE PROCEDURE  [dbo].[proc_INTERNATIONAL_CODE_Save]
	@international_code_PK int = NULL,
	@sourcecode nvarchar(60) = NULL,
	@resolutionmode_sources int = NULL,
	@referencetext ntext = NULL,
	@resolutionmode_substance int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[INTERNATIONAL_CODE]
	SET
	[sourcecode] = @sourcecode,
	[resolutionmode_sources] = @resolutionmode_sources,
	[referencetext] = @referencetext,
	[resolutionmode_substance] = @resolutionmode_substance
	WHERE [international_code_PK] = @international_code_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[INTERNATIONAL_CODE]
		([sourcecode], [resolutionmode_sources], [referencetext], [resolutionmode_substance])
		VALUES
		(@sourcecode, @resolutionmode_sources, @referencetext, @resolutionmode_substance)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
