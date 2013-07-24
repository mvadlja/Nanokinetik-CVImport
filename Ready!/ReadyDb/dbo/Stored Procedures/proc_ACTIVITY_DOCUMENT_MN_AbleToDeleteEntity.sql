
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_AbleToDeleteEntity]
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
		FROM [dbo].[ACTIVITY_DOCUMENT_MN] activityDocumentMn
		WHERE (activityDocumentMn.document_FK = @documentPk OR @documentPk IS NULL)
	
		SET @NumberOfActivities = ISNULL (@NumberOfActivities, 0);
		
		IF (@NumberOfActivities = 1)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END