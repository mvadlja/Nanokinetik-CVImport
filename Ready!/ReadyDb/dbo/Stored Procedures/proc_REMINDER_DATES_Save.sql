
-- Save
CREATE PROCEDURE [dbo].[proc_REMINDER_DATES_Save]
	@reminder_date_PK int = NULL,
	@reminder_date datetime = NULL,
	@reminder_repeating_mode_FK int = NULL,
	@reminder_status_FK int = NULL,
	@reminder_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[REMINDER_DATES]
	SET
	[reminder_date] = @reminder_date,
	[reminder_repeating_mode_FK] = @reminder_repeating_mode_FK,
	[reminder_status_FK] = @reminder_status_FK,
	[reminder_FK] = @reminder_FK
	WHERE [reminder_date_PK] = @reminder_date_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[REMINDER_DATES]
		([reminder_date], [reminder_repeating_mode_FK], [reminder_status_FK], [reminder_FK])
		VALUES
		(@reminder_date, @reminder_repeating_mode_FK, @reminder_status_FK, @reminder_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END