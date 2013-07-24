-- Save
CREATE PROCEDURE  [dbo].[proc_PROJECT_SAVED_SEARCH_Save]
	@project_saved_search_PK int = NULL,
	@name nvarchar(150) = NULL,
	@user_FK int = NULL,
	@internal_status_type_FK int = NULL,
	@country_FK int = NULL,
	@start_date_from date = NULL,
	@start_date_to date = NULL,
	@expected_finished_date_from date = NULL,
	@expected_finished_dat_to date = NULL,
	@actual_finished_date_from date = NULL,
	@actual_finished_date_to date = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK1 int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PROJECT_SAVED_SEARCH]
	SET
	[name] = @name,
	[user_FK] = @user_FK,
	[internal_status_type_FK] = @internal_status_type_FK,
	[country_FK] = @country_FK,
	[start_date_from] = @start_date_from,
	[start_date_to] = @start_date_to,
	[expected_finished_date_from] = @expected_finished_date_from,
	[expected_finished_dat_to] = @expected_finished_dat_to,
	[actual_finished_date_from] = @actual_finished_date_from,
	[actual_finished_date_to] = @actual_finished_date_to,
	[displayName] = @displayName,
	[user_FK1] = @user_FK1,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic
	WHERE [project_saved_search_PK] = @project_saved_search_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PROJECT_SAVED_SEARCH]
		([name], [user_FK], [internal_status_type_FK], [country_FK], [start_date_from], [start_date_to], [expected_finished_date_from], [expected_finished_dat_to], [actual_finished_date_from], [actual_finished_date_to], [displayName], [user_FK1], [gridLayout], [isPublic])
		VALUES
		(@name, @user_FK, @internal_status_type_FK, @country_FK, @start_date_from, @start_date_to, @expected_finished_date_from, @expected_finished_dat_to, @actual_finished_date_from, @actual_finished_date_to, @displayName, @user_FK1, @gridLayout, @isPublic)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
