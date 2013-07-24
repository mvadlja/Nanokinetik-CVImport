
-- GetEntity
CREATE PROCEDURE [dbo].[proc_REMINDER_REPEATING_MODES_GetEntity]
	@reminder_repeating_mode_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_repeating_mode_PK], [name], [enum_name]
	FROM [dbo].[REMINDER_REPEATING_MODES]
	WHERE [reminder_repeating_mode_PK] = @reminder_repeating_mode_PK
END