-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PROJECT_MN_Delete]
	@activity_project_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_PROJECT_MN] WHERE [activity_project_PK] = @activity_project_PK
END
