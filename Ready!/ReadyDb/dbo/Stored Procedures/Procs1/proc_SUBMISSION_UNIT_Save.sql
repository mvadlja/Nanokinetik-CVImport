-- Save
CREATE PROCEDURE  proc_SUBMISSION_UNIT_Save
	@subbmission_unit_PK int = NULL,
	@task_FK int = NULL,
	@submission_ID nvarchar(200) = NULL,
	@agency_role_FK int = NULL,
	@comment nvarchar(MAX) = NULL,
	@s_format_FK int = NULL,
	@description_type_FK int = NULL,
	@dispatch_date datetime = NULL,
	@receipt_date datetime = NULL,
	@sequence nvarchar(100) = NULL,
	@dtd_schema_FK nchar(10) = NULL,
	@organization_agency_FK int = NULL,
	@document_FK int = NULL,
	@ness_FK int = NULL,
	@ectd_FK int = NULL,
	@person_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBMISSION_UNIT]
	SET
	[task_FK] = @task_FK,
	[submission_ID] = @submission_ID,
	[agency_role_FK] = @agency_role_FK,
	[comment] = @comment,
	[s_format_FK] = @s_format_FK,
	[description_type_FK] = @description_type_FK,
	[dispatch_date] = @dispatch_date,
	[receipt_date] = @receipt_date,
	[sequence] = @sequence,
	[dtd_schema_FK] = @dtd_schema_FK,
	[organization_agency_FK] = @organization_agency_FK,
	[document_FK] = @document_FK,
	[ness_FK] = @ness_FK,
	[ectd_FK] = @ectd_FK,
	[person_FK] = @person_FK
	WHERE [subbmission_unit_PK] = @subbmission_unit_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBMISSION_UNIT]
		([task_FK], [submission_ID], [agency_role_FK], [comment], [s_format_FK], [description_type_FK], [dispatch_date], [receipt_date], [sequence], [dtd_schema_FK], [organization_agency_FK], [document_FK], [ness_FK], [ectd_FK], [person_FK])
		VALUES
		(@task_FK, @submission_ID, @agency_role_FK, @comment, @s_format_FK, @description_type_FK, @dispatch_date, @receipt_date, @sequence, @dtd_schema_FK, @organization_agency_FK, @document_FK, @ness_FK, @ectd_FK, @person_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
