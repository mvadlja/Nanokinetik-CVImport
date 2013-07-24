-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetAssignedEntitiesByActivity]
	@ActivityPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	p.*
	FROM [dbo].ACTIVITY_PROJECT_MN apMn
	JOIN dbo.PROJECT p ON p.project_PK = apMn.project_FK
	WHERE apMn.activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL
END