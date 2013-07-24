-- Save
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_PROJECT_MN_Save]
	@activity_project_PK int = NULL,
	@activity_FK int = NULL,
	@project_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY_PROJECT_MN]
	SET
	[activity_FK] = @activity_FK,
	[project_FK] = @project_FK
	WHERE [activity_project_PK] = @activity_project_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY_PROJECT_MN]
		([activity_FK], [project_FK])
		VALUES
		(@activity_FK, @project_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
