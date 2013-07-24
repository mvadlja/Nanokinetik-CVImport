-- Save
CREATE PROCEDURE  [dbo].[proc_PROJECT_Save]
	@project_PK int = NULL,
	@user_FK int = NULL,
	@name nvarchar(150) = NULL,
	@comment nvarchar(MAX) = NULL,
	@start_date datetime = NULL,
	@expected_finished_date datetime = NULL,
	@actual_finished_date datetime = NULL,
	@description nvarchar(MAX) = NULL,
	@internal_status_type_FK int = NULL,
	@automatic_alerts_on bit = 1,
	@prevent_start_date_alert bit =0,
	@prevent_exp_finish_date_alert bit = 0
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PROJECT]
	SET
	[user_FK] = @user_FK,
	[name] = @name,
	[comment] = @comment,
	[start_date] = @start_date,
	[expected_finished_date] = @expected_finished_date,
	[actual_finished_date] = @actual_finished_date,
	[description] = @description,
	[internal_status_type_FK] = @internal_status_type_FK,
	[automatic_alerts_on] = @automatic_alerts_on,
	[prevent_start_date_alert] = @prevent_start_date_alert,
	[prevent_exp_finish_date_alert] = @prevent_exp_finish_date_alert
	WHERE [project_PK] = @project_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PROJECT]
		([user_FK], [name], [comment], [start_date], [expected_finished_date], [actual_finished_date], [description], [internal_status_type_FK],[automatic_alerts_on],[prevent_start_date_alert], [prevent_exp_finish_date_alert])
		VALUES
		(@user_FK, @name, @comment, @start_date, @expected_finished_date, @actual_finished_date, @description, @internal_status_type_FK, @automatic_alerts_on, @prevent_start_date_alert,@prevent_exp_finish_date_alert)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
