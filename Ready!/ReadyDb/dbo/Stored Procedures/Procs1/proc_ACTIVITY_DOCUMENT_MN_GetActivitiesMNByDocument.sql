-- GetActivitiesMNByDocument
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_GetActivitiesMNByDocument]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[ACTIVITY_DOCUMENT_MN] mn
	WHERE (mn.document_FK = @document_FK OR @document_FK IS NULL)

END
