-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetEntitiesByActivity]
	@ActivityPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	dbo.PROJECT.*
	FROM [dbo].ACTIVITY_PROJECT_MN
	JOIN [dbo].PROJECT ON [dbo].PROJECT.project_PK = [dbo].ACTIVITY_PROJECT_MN.project_FK
	WHERE [dbo].ACTIVITY_PROJECT_MN.activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL
END