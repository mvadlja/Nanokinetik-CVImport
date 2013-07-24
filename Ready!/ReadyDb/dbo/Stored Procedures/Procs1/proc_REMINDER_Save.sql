-- Save
CREATE PROCEDURE  [dbo].[proc_REMINDER_Save]
	@reminder_PK int = NULL,
	@user_FK int = NULL,
	@responsible_user_FK int = NULL,
	@table_name nvarchar(100) = NULL,
	@entity_FK int = NULL,
	@related_attribute_name nvarchar(50) = NULL,
	@related_attribute_value nvarchar(MAX) = NULL,
	@reminder_name nvarchar(500) = NULL,
	@reminder_type nvarchar(500) = NULL,
	@navigate_url nvarchar(100) = NULL,
	@time_before_activation bigint = NULL,
	@remind_me_on_email bit = NULL,
	@additional_emails nvarchar(MAX) = NULL,
	@description nvarchar(MAX) = NULL,
	@is_automatic bit = NULL,
	@related_entity_FK int = NULL,
	@reminder_user_status_FK int = NULL,
	@comment nvarchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[REMINDER]
	SET
	[user_FK] = @user_FK,
	[responsible_user_FK] = @responsible_user_FK,
	[table_name] = @table_name,
	[entity_FK] = @entity_FK,
	[related_attribute_name] = @related_attribute_name,
	[related_attribute_value] = @related_attribute_value,
	[reminder_name] = @reminder_name,
	[reminder_type] = @reminder_type,
	[navigate_url] = @navigate_url,
	[time_before_activation] = @time_before_activation,
	[remind_me_on_email] = @remind_me_on_email,
	[additional_emails] = @additional_emails,
	[description] = @description,
	[is_automatic] = @is_automatic,
	[related_entity_FK] = @related_entity_FK,
	[reminder_user_status_FK] = @reminder_user_status_FK, 
	[comment] = @comment
	WHERE [reminder_PK] = @reminder_PK
	

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[REMINDER]
		([user_FK], [responsible_user_FK], [table_name], [entity_FK], [related_attribute_name], [related_attribute_value], [reminder_name], [reminder_type], [navigate_url], [time_before_activation], [remind_me_on_email], [additional_emails], [description], [is_automatic], [related_entity_FK], [reminder_user_status_FK], [comment])
		VALUES
		(@user_FK, @responsible_user_FK, @table_name, @entity_FK, @related_attribute_name, @related_attribute_value, @reminder_name, @reminder_type, @navigate_url, @time_before_activation, @remind_me_on_email, @additional_emails, @description, @is_automatic, @related_entity_FK, @reminder_user_status_FK, @comment)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
