﻿
-- GetEntity
CREATE PROCEDURE [dbo].[proc_REMINDER_DATES_GetEntity]
	@reminder_date_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_date_PK], [reminder_date], [reminder_repeating_mode_FK], [reminder_status_FK], [reminder_FK] 
	FROM [dbo].[REMINDER_DATES]
	WHERE [reminder_date_PK] = @reminder_date_PK
END