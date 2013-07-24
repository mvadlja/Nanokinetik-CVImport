
CREATE PROCEDURE  [dbo].[proc_TASK_AbleToDeleteEntity]
	@TaskPk int = NULL
AS
DECLARE @NumSubUnit INT = NULL;
DECLARE @NumDoc INT = NULL;
BEGIN
	SET NOCOUNT ON;

	IF (@TaskPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumSubUnit = COUNT(su.subbmission_unit_PK)
		FROM dbo.SUBMISSION_UNIT su
		WHERE su.task_FK = @TaskPk AND @TaskPk IS NOT NULL

		SELECT @NumDoc = COUNT(tdMn.document_FK)
		FROM dbo.TASK_DOCUMENT_MN tdMn
		WHERE tdMn.task_FK = @TaskPk AND @TaskPk IS NOT NULL

		SET @NumSubUnit = ISNULL (@NumSubUnit, 0);
		SET @NumDoc = ISNULL (@NumDoc, 0);

		IF ((@NumSubUnit + @NumDoc) = 0)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END