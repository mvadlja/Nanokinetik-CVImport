-- Save
CREATE PROCEDURE  [dbo].[proc_TASK_Save]
	@task_PK int = NULL,
	@activity_FK int = NULL,
	@user_FK int = NULL,
	@task_name_FK int = NULL,
	@description nvarchar(MAX) = NULL,
	@comment nvarchar(MAX) = NULL,
	@type_internal_status_FK int = NULL,
	@start_date datetime = NULL,
	@expected_finished_date date = NULL,
	@actual_finished_date datetime = NULL,
	@POM_internal_status int = NULL,
	@automatic_alerts_on bit = 1,
	@prevent_start_date_alert bit = 0,
	@prevent_exp_finish_date_alert bit = 0,
	@task_ID nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TASK]
	SET
	[activity_FK] = @activity_FK,
	[user_FK] = @user_FK,
	[task_name_FK] = @task_name_FK,
	[description] = @description,
	[comment] = @comment,
	[type_internal_status_FK] = @type_internal_status_FK,
	[start_date] = @start_date,
	[expected_finished_date] = @expected_finished_date,
	[actual_finished_date] = @actual_finished_date,
	[POM_internal_status] = @POM_internal_status,
	[automatic_alerts_on] = @automatic_alerts_on,
	[prevent_start_date_alert] = @prevent_start_date_alert, 
	[prevent_exp_finish_date_alert] = @prevent_exp_finish_date_alert,
	[task_ID] = @task_ID
	WHERE [task_PK] = @task_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TASK]
		([activity_FK], [user_FK], [task_name_FK], [description], [comment], [type_internal_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [POM_internal_status],[automatic_alerts_on],[prevent_start_date_alert], [prevent_exp_finish_date_alert], [task_ID] )
		VALUES
		(@activity_FK, @user_FK, @task_name_FK, @description, @comment, @type_internal_status_FK, @start_date, @expected_finished_date, @actual_finished_date, @POM_internal_status, @automatic_alerts_on, @prevent_start_date_alert, @prevent_exp_finish_date_alert, @task_ID )

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
