-- Save
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_SAVED_SEARCH_Save]
	@time_unit_saved_search_PK int = NULL,
	@activity_FK int = NULL,
	@time_unit_FK int = NULL,
	@user_FK int = NULL,
	@actual_date_from date = NULL,
	@actual_date_to date = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK1 int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TIME_UNIT_SAVED_SEARCH]
	SET
	[activity_FK] = @activity_FK,
	[time_unit_FK] = @time_unit_FK,
	[user_FK] = @user_FK,
	[actual_date_from] = @actual_date_from,
	[actual_date_to] = @actual_date_to,
	[displayName] = @displayName,
	[user_FK1] = @user_FK1,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic
	WHERE [time_unit_saved_search_PK] = @time_unit_saved_search_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TIME_UNIT_SAVED_SEARCH]
		([activity_FK], [time_unit_FK], [user_FK], [actual_date_from], [actual_date_to], [displayName], [user_FK1], [gridLayout], [isPublic])
		VALUES
		(@activity_FK, @time_unit_FK, @user_FK, @actual_date_from, @actual_date_to, @displayName, @user_FK1, @gridLayout, @isPublic)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
