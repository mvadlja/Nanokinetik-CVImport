-- Save
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ATTACHMENT_Save]
	@substance_attachment_PK int = NULL,
	@attachmentreference nvarchar(60) = NULL,
	@resolutionmode int = NULL,
	@validitydeclaration int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_ATTACHMENT]
	SET
	[attachmentreference] = @attachmentreference,
	[resolutionmode] = @resolutionmode,
	[validitydeclaration] = @validitydeclaration
	WHERE [substance_attachment_PK] = @substance_attachment_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_ATTACHMENT]
		([attachmentreference], [resolutionmode], [validitydeclaration])
		VALUES
		(@attachmentreference, @resolutionmode, @validitydeclaration)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
