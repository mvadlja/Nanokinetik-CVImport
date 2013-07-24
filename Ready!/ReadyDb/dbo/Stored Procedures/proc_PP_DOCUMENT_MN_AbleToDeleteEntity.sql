
CREATE PROCEDURE  [dbo].[proc_PP_DOCUMENT_MN_AbleToDeleteEntity]
	@documentPk int = NULL
AS
DECLARE @NumberOfActivities INT = NULL;

BEGIN
	SET NOCOUNT ON;

	IF (@documentPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumberOfActivities = COUNT(*)
		FROM [dbo].[pP_DOCUMENT_MN] ppDocumentMn
		WHERE (ppDocumentMn.doc_FK = @documentPk OR @documentPk IS NULL)
	
		SET @NumberOfActivities = ISNULL (@NumberOfActivities, 0);

		IF (@NumberOfActivities = 1)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END