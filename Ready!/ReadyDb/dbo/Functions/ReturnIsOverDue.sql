create FUNCTION  [dbo].[ReturnIsOverDue]
( 
	@reminder_FK int = NULL
)

RETURNS NVARCHAR(5)
AS
BEGIN

	DECLARE @numberOfOverDueDates INT = 0;
	declare @result nvarchar(max);
	
	IF (@reminder_FK = NULL)
		SET @result = 'Yes';
	
	ELSE
	BEGIN
		SELECT @numberOfOverDueDates = COUNT(*)
		FROM [dbo].[REMINDER] reminder
		LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
		LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
		WHERE reminder.reminder_PK = @reminder_FK AND reminderDates.reminder_date < GETDATE()
						
		IF (@numberOfOverDueDates > 0)
			SET @result = 'Yes';
		ELSE
			SET @result = 'No';
			
	END
	
	RETURN @result;	
END