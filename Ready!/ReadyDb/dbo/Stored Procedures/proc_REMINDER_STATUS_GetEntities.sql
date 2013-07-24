
-- GetEntities
CREATE PROCEDURE [dbo].[proc_REMINDER_STATUS_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_status_PK], [name], [enum_name]
	FROM [dbo].[REMINDER_STATUS]
END