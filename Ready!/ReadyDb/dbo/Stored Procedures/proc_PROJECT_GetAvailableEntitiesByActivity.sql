-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetAvailableEntitiesByActivity]
	@ActivityPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	dbo.PROJECT.*
	FROM [dbo].PROJECT
	WHERE [dbo].PROJECT.project_PK NOT IN
	(	
		SELECT project_FK 
		FROM dbo.ACTIVITY_PROJECT_MN 
		WHERE activity_FK = @ActivityPk AND @ActivityPk IS NOT NULL AND project_FK is not null
	)
END