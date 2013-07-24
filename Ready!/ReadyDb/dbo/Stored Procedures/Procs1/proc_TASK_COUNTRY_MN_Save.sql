-- Save
CREATE PROCEDURE  [dbo].[proc_TASK_COUNTRY_MN_Save]
	@task_country_PK int = NULL,
	@task_FK int = NULL,
	@country_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TASK_COUNTRY_MN]
	SET
	[task_FK] = @task_FK,
	[country_FK] = @country_FK
	WHERE [task_country_PK] = @task_country_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TASK_COUNTRY_MN]
		([task_FK], [country_FK])
		VALUES
		(@task_FK, @country_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
