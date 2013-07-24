-- Save
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_SAVED_SEARCH_Save]
	@activity_saved_search_PK int = NULL,
	@project_FK int = NULL,
	@product_FK int = NULL,
	@name nvarchar(350) = NULL,
	@user_FK int = NULL,
	@procedure_number nvarchar(250) = NULL,
	@procedure_type_FK int = NULL,
	@type_FK int = NULL,
	@regulatory_status_FK int = NULL,
	@internal_status_FK int = NULL,
	@activity_mode_FK int = NULL,
	@applicant_FK int = NULL,
	@country_FK int = NULL,
	@legal nvarchar(150) = NULL,
	@start_date_from date = NULL,
	@start_date_to date = NULL,
	@expected_finished_date_from date = NULL,
	@expected_finished_date_to date = NULL,
	@actual_finished_date_from date = NULL,
	@actual_finished_date_to date = NULL,
	@approval_date_from date = NULL,
	@approval_date_to date = NULL,
	@submission_date_from date = NULL,
	@submission_date_to date = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK1 int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL, 
	@billable bit = NULL,
	@activity_id nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY_SAVED_SEARCH]
	SET
	[project_FK] = @project_FK,
	[product_FK] = @product_FK,
	[name] = @name,
	[user_FK] = @user_FK,
	[procedure_number] = @procedure_number,
	[procedure_type_FK] = @procedure_type_FK,
	[type_FK] = @type_FK,
	[regulatory_status_FK] = @regulatory_status_FK,
	[internal_status_FK] = @internal_status_FK,
	[activity_mode_FK] = @activity_mode_FK,
	[applicant_FK] = @applicant_FK,
	[country_FK] = @country_FK,
	[legal] = @legal,
	[start_date_from] = @start_date_from,
	[start_date_to] = @start_date_to,
	[expected_finished_date_from] = @expected_finished_date_from,
	[expected_finished_date_to] = @expected_finished_date_to,
	[actual_finished_date_from] = @actual_finished_date_from,
	[actual_finished_date_to] = @actual_finished_date_to,
	[approval_date_from] = @approval_date_from,
	[approval_date_to] = @approval_date_to,
	[submission_date_from] = @submission_date_from,
	[submission_date_to] = @submission_date_to,
	[displayName] = @displayName,
	[user_FK1] = @user_FK1,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic, 
	[billable] = @billable,
	[activity_ID] = @activity_id
	WHERE [activity_saved_search_PK] = @activity_saved_search_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY_SAVED_SEARCH]
		([project_FK], [product_FK], [name], [user_FK], [procedure_number], [procedure_type_FK], [type_FK], [regulatory_status_FK], [internal_status_FK], [activity_mode_FK], [applicant_FK], [country_FK], [legal], [start_date_from], [start_date_to], [expected_finished_date_from], [expected_finished_date_to], [actual_finished_date_from], [actual_finished_date_to], [approval_date_from], [approval_date_to], [submission_date_from], [submission_date_to], [displayName], [user_FK1], [gridLayout], [isPublic], [billable], [activity_ID])
		VALUES
		(@project_FK, @product_FK, @name, @user_FK, @procedure_number, @procedure_type_FK, @type_FK, @regulatory_status_FK, @internal_status_FK, @activity_mode_FK, @applicant_FK, @country_FK, @legal, @start_date_from, @start_date_to, @expected_finished_date_from, @expected_finished_date_to, @actual_finished_date_from, @actual_finished_date_to, @approval_date_from, @approval_date_to, @submission_date_from, @submission_date_to, @displayName, @user_FK1, @gridLayout, @isPublic, @billable, @activity_id)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
