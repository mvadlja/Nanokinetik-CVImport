-- Save
CREATE PROCEDURE  [dbo].[proc_TASK_NAME_Save]
	@task_name_PK int = NULL,
	@task_name nvarchar(150) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TASK_NAME]
	SET
	[task_name] = @task_name
	WHERE [task_name_PK] = @task_name_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TASK_NAME]
		([task_name])
		VALUES
		(@task_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
