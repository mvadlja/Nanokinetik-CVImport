-- Save
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_Save]
	@substance_alias_PK int = NULL,
	@sourcecode nvarchar(60) = NULL,
	@resolutionmode int = NULL,
	@aliasname ntext = NULL,
	@substance_aliastranslations_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_ALIAS]
	SET
	[sourcecode] = @sourcecode,
	[resolutionmode] = @resolutionmode,
	[aliasname] = @aliasname,
	[substance_aliastranslations_FK] = @substance_aliastranslations_FK
	WHERE [substance_alias_PK] = @substance_alias_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_ALIAS]
		([sourcecode], [resolutionmode], [aliasname], [substance_aliastranslations_FK])
		VALUES
		(@sourcecode, @resolutionmode, @aliasname, @substance_aliastranslations_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
