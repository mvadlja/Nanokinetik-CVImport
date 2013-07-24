-- Delete
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetActiveRemindersForUser]
	@user_PK int = null
AS
declare @person_FK int = null;
declare @numOverDue int = null;
BEGIN
	
	select @person_FK = dbo.[USER].person_FK
	from dbo.[USER]
	where dbo.[USER].user_PK = @user_PK

	select @numOverDue = COUNT(*) FROM [dbo].[REMINDER] reminder
	LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
	LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
	WHERE responsible_user_FK = @person_FK
	and (reminderStatus.name = 'Active' or reminderStatus.name = 'EmailSent')
	and reminder_date <= GETDATE()--DATEADD(day,- isnull(time_before_activation,0),reminder_date) <= GETDATE()

	select (
	SELECT COUNT(*) FROM [dbo].[REMINDER] reminder
	LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
	LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
	WHERE responsible_user_FK = @person_FK
	and (reminderStatus.name = 'Active' or reminderStatus.name = 'EmailSent')) as NumReminders,
	case when @numOverDue is not null and @numOverDue > 0 then 'true'
	else 'false'
	end as OverDue
END
