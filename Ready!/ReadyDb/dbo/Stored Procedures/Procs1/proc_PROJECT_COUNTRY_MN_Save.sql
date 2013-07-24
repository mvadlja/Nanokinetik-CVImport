-- Save
CREATE PROCEDURE  [dbo].[proc_PROJECT_COUNTRY_MN_Save]
	@project_country_PK int = NULL,
	@project_FK int = NULL,
	@country_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PROJECT_COUNTRY_MN]
	SET
	[project_FK] = @project_FK,
	[country_FK] = @country_FK
	WHERE [project_country_PK] = @project_country_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PROJECT_COUNTRY_MN]
		([project_FK], [country_FK])
		VALUES
		(@project_FK, @country_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
