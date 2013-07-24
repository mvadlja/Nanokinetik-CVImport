
-- Save
CREATE PROCEDURE [dbo].[proc_ALERT_SAVED_SEARCH_Save]
	@alert_saved_search_PK int = NULL,
	@product_FK int = NULL,
	@ap_FK int = NULL,
	@project_FK int = NULL,
	@activity_FK int = NULL,
	@task_FK int = NULL,
	@document_FK int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL,
	@name nvarchar(250) = NULL,
	@reminder_repeating_mode_FK int = NULL,
	@send_mail bit = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ALERT_SAVED_SEARCH]
	SET
	[product_FK] = @product_FK,
	[ap_FK] = @ap_FK,
	[project_FK] = @project_FK,
	[activity_FK] = @activity_FK,
	[task_FK] = @task_FK,
	[document_FK] = @document_FK,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic,
	[name] = @name,
	[reminder_repeating_mode_FK] = @reminder_repeating_mode_FK,
	[send_mail] = @send_mail,
	[displayName] = @displayName,
	[user_FK] = @user_FK
	WHERE [alert_saved_search_PK] = @alert_saved_search_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ALERT_SAVED_SEARCH]
		([product_FK], [ap_FK], [project_FK], [activity_FK], [task_FK], [document_FK], [gridLayout], [isPublic], [name], [reminder_repeating_mode_FK], [send_mail], [displayName], [user_FK])
		VALUES
		(@product_FK, @ap_FK, @project_FK, @activity_FK, @task_FK, @document_FK, @gridLayout, @isPublic, @name, @reminder_repeating_mode_FK, @send_mail, @displayName, @user_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END