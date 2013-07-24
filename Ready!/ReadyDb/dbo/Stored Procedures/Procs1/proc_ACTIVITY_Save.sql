-- Save
CREATE PROCEDURE  proc_ACTIVITY_Save
	@activity_PK int = NULL,
	@user_FK int = NULL,
	@mode_FK int = NULL,
	@procedure_type_FK int = NULL,
	@name nvarchar(350) = NULL,
	@description nvarchar(MAX) = NULL,
	@comment nvarchar(MAX) = NULL,
	@regulatory_status_FK int = NULL,
	@start_date date = NULL,
	@expected_finished_date date = NULL,
	@actual_finished_date datetime = NULL,
	@approval_date datetime = NULL,
	@submission_date datetime = NULL,
	@procedure_number nvarchar(250) = NULL,
	@legal nvarchar(150) = NULL,
	@cost nvarchar(150) = NULL,
	@internal_status_FK int = NULL,
	@activity_ID nvarchar(100) = NULL,
	@automatic_alerts_on bit = 1,
	@prevent_start_date_alert bit = 0,
	@prevent_exp_finish_date_alert bit = 0,
	@billable bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY]
	SET
	[user_FK] = @user_FK,
	[mode_FK] = @mode_FK,
	[procedure_type_FK] = @procedure_type_FK,
	[name] = @name,
	[description] = @description,
	[comment] = @comment,
	[regulatory_status_FK] = @regulatory_status_FK,
	[start_date] = @start_date,
	[expected_finished_date] = @expected_finished_date,
	[actual_finished_date] = @actual_finished_date,
	[approval_date] = @approval_date,
	[submission_date] = @submission_date,
	[procedure_number] = @procedure_number,
	[legal] = @legal,
	[cost] = @cost,
	[internal_status_FK] = @internal_status_FK,
	[activity_ID] = @activity_ID,
	[automatic_alerts_on] = @automatic_alerts_on,
	[prevent_start_date_alert] = @prevent_start_date_alert,
	[prevent_exp_finish_date_alert] = @prevent_exp_finish_date_alert,
	[billable] = @billable
	WHERE [activity_PK] = @activity_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY]
		([user_FK], [mode_FK], [procedure_type_FK], [name], [description], [comment], [regulatory_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [approval_date], [submission_date], [procedure_number], [legal], [cost], [internal_status_FK], [activity_ID], [automatic_alerts_on],[prevent_start_date_alert],[prevent_exp_finish_date_alert], [billable])
		VALUES
		(@user_FK, @mode_FK, @procedure_type_FK, @name, @description, @comment, @regulatory_status_FK, @start_date, @expected_finished_date, @actual_finished_date, @approval_date, @submission_date, @procedure_number, @legal, @cost, @internal_status_FK, @activity_ID, @automatic_alerts_on, @prevent_start_date_alert, @prevent_exp_finish_date_alert, @billable)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
