-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ATTACHMENT_GetEntity]
	@substance_attachment_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_attachment_PK], [attachmentreference], [resolutionmode], [validitydeclaration]
	FROM [dbo].[SUBSTANCE_ATTACHMENT]
	WHERE [substance_attachment_PK] = @substance_attachment_PK
END
