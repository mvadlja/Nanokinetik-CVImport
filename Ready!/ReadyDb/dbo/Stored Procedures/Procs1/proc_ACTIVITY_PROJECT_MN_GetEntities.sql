-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PROJECT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_project_PK], [activity_FK], [project_FK]
	FROM [dbo].[ACTIVITY_PROJECT_MN]
END
