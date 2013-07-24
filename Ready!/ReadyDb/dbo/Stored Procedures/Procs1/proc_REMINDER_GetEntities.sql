-- GetEntities
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_PK], [user_FK], [responsible_user_FK], [table_name], [entity_FK], [related_attribute_name], [related_attribute_value], [reminder_name], [reminder_type], [navigate_url], [time_before_activation], [remind_me_on_email], [additional_emails], [description], [is_automatic], [related_entity_FK], [reminder_user_status_FK], [comment]
	FROM [dbo].[REMINDER]
END
