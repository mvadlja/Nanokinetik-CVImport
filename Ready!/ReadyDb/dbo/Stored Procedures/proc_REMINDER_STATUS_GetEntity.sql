
-- GetEntity
CREATE PROCEDURE [dbo].[proc_REMINDER_STATUS_GetEntity]
	@reminder_status_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_status_PK], [name], [enum_name]
	FROM [dbo].[REMINDER_STATUS]
	WHERE [reminder_status_PK] = @reminder_status_PK
END