-- Delete
CREATE PROCEDURE  [dbo].[proc_PROJECT_Delete]
	@project_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PROJECT] WHERE [project_PK] = @project_PK
END
