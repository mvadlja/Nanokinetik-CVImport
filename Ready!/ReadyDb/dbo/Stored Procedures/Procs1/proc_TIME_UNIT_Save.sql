-- Save
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_Save]
	@time_unit_PK int = NULL,
	@task_FK int = NULL,
	@user_FK int = NULL,
	@time_unit_name_FK int = NULL,
	@description nvarchar(MAX) = NULL,
	@comment nvarchar(MAX) = NULL,
	@actual_date datetime = NULL,
	@time_hours int = NULL,
	@time_minutes int = NULL,
	@activity_FK int = NULL,
	@inserted_by int = NULL,
	@Name nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TIME_UNIT]
	SET
	[task_FK] = @task_FK,
	[user_FK] = @user_FK,
	[time_unit_name_FK] = @time_unit_name_FK,
	[description] = @description,
	[comment] = @comment,
	[actual_date] = @actual_date,
	[time_hours] = @time_hours,
	[time_minutes] = @time_minutes,
	[activity_FK] = @activity_FK,
	[inserted_by] = @inserted_by
	WHERE [time_unit_PK] = @time_unit_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TIME_UNIT]
		([task_FK], [user_FK], [time_unit_name_FK], [description], [comment], [actual_date], [time_hours], [time_minutes], [activity_FK], [inserted_by])
		VALUES
		(@task_FK, @user_FK, @time_unit_name_FK, @description, @comment, @actual_date, @time_hours, @time_minutes, @activity_FK, @inserted_by)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
