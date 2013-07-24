-- Delete
CREATE PROCEDURE  [dbo].[proc_PROJECT_COUNTRY_MN_DeleteByProjectId]
	@projectId int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PROJECT_COUNTRY_MN] WHERE project_FK = @projectId
END
