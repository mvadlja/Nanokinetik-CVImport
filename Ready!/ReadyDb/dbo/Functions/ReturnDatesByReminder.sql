
CREATE FUNCTION [dbo].[ReturnDatesByReminder]
(
	@reminder_FK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @Dates nvarchar(max)

	SELECT @Dates = COALESCE(@Dates + ', ', '') + isnull(rtrim(ltrim(CONVERT(nvarchar(12), reminderDates.reminder_date, 104))), '')
			
	FROM [dbo].[REMINDER] reminder
	LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
	LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
	WHERE reminder.reminder_PK = @reminder_FK 
	
    RETURN @Dates
    
  END