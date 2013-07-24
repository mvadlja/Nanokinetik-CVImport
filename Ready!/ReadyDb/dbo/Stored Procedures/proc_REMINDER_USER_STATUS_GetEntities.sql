
-- GetEntities
CREATE PROCEDURE [dbo].[proc_REMINDER_USER_STATUS_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_user_status_PK], [name], [enum_name]
	FROM [dbo].[REMINDER_USER_STATUS]
END