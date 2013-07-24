-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ATTACHMENT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[substance_attachment_PK], [attachmentreference], [resolutionmode], [validitydeclaration]
	FROM [dbo].[SUBSTANCE_ATTACHMENT]
END
