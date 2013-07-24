-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_document_PK], [activity_FK], [document_FK]
	FROM [dbo].[ACTIVITY_DOCUMENT_MN]
END
