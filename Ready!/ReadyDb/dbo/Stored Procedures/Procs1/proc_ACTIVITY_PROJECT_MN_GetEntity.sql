-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PROJECT_MN_GetEntity]
	@activity_project_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_project_PK], [activity_FK], [project_FK]
	FROM [dbo].[ACTIVITY_PROJECT_MN]
	WHERE [activity_project_PK] = @activity_project_PK
END
