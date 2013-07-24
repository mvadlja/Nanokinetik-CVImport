-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_GetEntity]
	@activity_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_document_PK], [activity_FK], [document_FK]
	FROM [dbo].[ACTIVITY_DOCUMENT_MN]
	WHERE [activity_document_PK] = @activity_document_PK
END
