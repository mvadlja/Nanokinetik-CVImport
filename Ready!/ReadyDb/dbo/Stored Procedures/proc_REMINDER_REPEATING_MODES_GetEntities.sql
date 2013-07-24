
-- GetEntities
CREATE PROCEDURE [dbo].[proc_REMINDER_REPEATING_MODES_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_repeating_mode_PK], [name], [enum_name]
	FROM [dbo].[REMINDER_REPEATING_MODES]
END