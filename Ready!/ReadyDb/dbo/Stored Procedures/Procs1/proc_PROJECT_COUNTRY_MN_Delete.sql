-- Delete
CREATE PROCEDURE  [dbo].[proc_PROJECT_COUNTRY_MN_Delete]
	@project_country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PROJECT_COUNTRY_MN] WHERE [project_country_PK] = @project_country_PK
END
