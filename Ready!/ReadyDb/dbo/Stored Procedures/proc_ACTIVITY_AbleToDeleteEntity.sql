
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_AbleToDeleteEntity]
	@ActivityPk int = NULL
AS
DECLARE @NumTimeUnit INT = NULL;
DECLARE @NumTask INT = NULL;
DECLARE @NumDoc INT = NULL;
BEGIN
	SET NOCOUNT ON;

	IF (@ActivityPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SELECT @NumTimeUnit = COUNT([dbo].TIME_UNIT.time_unit_PK)
		FROM [dbo].TIME_UNIT
		WHERE [dbo].TIME_UNIT.activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL

		SELECT @NumTask = COUNT([dbo].TASK.task_PK)
		FROM dbo.TASK
		WHERE [dbo].TASK.activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL

		SELECT @NumDoc = COUNT([dbo].ACTIVITY_DOCUMENT_MN.document_FK)
		FROM dbo.ACTIVITY_DOCUMENT_MN
		WHERE [dbo].ACTIVITY_DOCUMENT_MN.activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL

		SET @NumTimeUnit = ISNULL (@NumTimeUnit, 0);
		SET @NumTask = ISNULL (@NumTask, 0);
		SET @NumDoc = ISNULL (@NumDoc, 0);

		IF ((@NumTimeUnit + @NumTask + @NumDoc) = 0)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END