
CREATE PROCEDURE  [dbo].[proc_PROJECT_AbleToDeleteEntity]
	@ProjectPk int = NULL
AS
DECLARE @NumDoc INT = NULL;
BEGIN
	SET NOCOUNT ON;

	IF (@ProjectPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumDoc = COUNT([dbo].PROJECT_DOCUMENT_MN.document_FK)
		FROM dbo.PROJECT_DOCUMENT_MN
		WHERE [dbo].PROJECT_DOCUMENT_MN.project_FK = @ProjectPk AND @ProjectPk IS NOT NULL

		SET @NumDoc = ISNULL (@NumDoc, 0);

		IF (@NumDoc = 0)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END