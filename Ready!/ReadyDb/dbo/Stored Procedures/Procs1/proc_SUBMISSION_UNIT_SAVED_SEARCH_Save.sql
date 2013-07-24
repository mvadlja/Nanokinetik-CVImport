-- Save
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_SAVED_SEARCH_Save]
	@submission_unit_saved_search_PK int = NULL,
	@product_FK int = NULL,
	@activity_FK int = NULL,
	@task_FK int = NULL,
	@description_type_FK int = NULL,
	@agency_FK int = NULL,
	@rms_FK int = NULL,
	@submission_ID nvarchar(200) = NULL,
	@s_format_FK int = NULL,
	@sequence nvarchar(100) = NULL,
	@dtd_schema_FK nchar(10) = NULL,
	@dispatch_date_from date = NULL,
	@dispatch_date_to date = NULL,
	@receipt_date_from date = NULL,
	@receipt_to date = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBMISSION_UNIT_SAVED_SEARCH]
	SET
	[product_FK] = @product_FK,
	[activity_FK] = @activity_FK,
	[task_FK] = @task_FK,
	[description_type_FK] = @description_type_FK,
	[agency_FK] = @agency_FK,
	[rms_FK] = @rms_FK,
	[submission_ID] = @submission_ID,
	[s_format_FK] = @s_format_FK,
	[sequence] = @sequence,
	[dtd_schema_FK] = @dtd_schema_FK,
	[dispatch_date_from] = @dispatch_date_from,
	[dispatch_date_to] = @dispatch_date_to,
	[receipt_date_from] = @receipt_date_from,
	[receipt_to] = @receipt_to,
	[displayName] = @displayName,
	[user_FK] = @user_FK,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic
	WHERE [submission_unit_saved_search_PK] = @submission_unit_saved_search_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBMISSION_UNIT_SAVED_SEARCH]
		([product_FK], [activity_FK], [task_FK], [description_type_FK], [agency_FK], [rms_FK], [submission_ID], [s_format_FK], [sequence], [dtd_schema_FK], [dispatch_date_from], [dispatch_date_to], [receipt_date_from], [receipt_to], [displayName], [user_FK], [gridLayout], [isPublic])
		VALUES
		(@product_FK, @activity_FK, @task_FK, @description_type_FK, @agency_FK, @rms_FK, @submission_ID, @s_format_FK, @sequence, @dtd_schema_FK, @dispatch_date_from, @dispatch_date_to, @receipt_date_from, @receipt_to, @displayName, @user_FK, @gridLayout, @isPublic)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
