
-- GetEntity
CREATE PROCEDURE [dbo].[proc_REMINDER_USER_STATUS_GetEntity]
	@reminder_user_status_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_user_status_PK], [name], [enum_name]
	FROM [dbo].[REMINDER_USER_STATUS]
	WHERE [reminder_user_status_PK] = @reminder_user_status_PK
END