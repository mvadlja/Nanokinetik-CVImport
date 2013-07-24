-- Save
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_SAVED_SEARCH_Save]
	@document_saved_search_PK int = NULL,
	@product_FK int = NULL,
	@ap_FK int = NULL,
	@project_FK int = NULL,
	@activity_FK int = NULL,
	@task_FK int = NULL,
	@name nvarchar(250) = NULL,
	@type_FK int = NULL,
	@version_number int = NULL,
	@version_label int = NULL,
	@document_number nvarchar(500) = NULL,
	@person_FK int = NULL,
	@regulatory_status int = NULL,
	@change_date_from date = NULL,
	@change_date_to date = NULL,
	@effective_start_date_from date = NULL,
	@effective_start_date_to date = NULL,
	@effective_end_date_from date = NULL,
	@effective_end_date_to date = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK1 int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL,
	@pp_FK int = NULL,
	@version_date_from date = NULL,
	@version_date_to date = NULL,
	@ev_code nvarchar(250) = NULL,
	@content nvarchar(250) = NULL, 
	@language_code nvarchar(250) = NULL, 
	@comments nvarchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[DOCUMENT_SAVED_SEARCH]
	SET
	[product_FK] = @product_FK,
	[ap_FK] = @ap_FK,
	[project_FK] = @project_FK,
	[activity_FK] = @activity_FK,
	[task_FK] = @task_FK,
	[name] = @name,
	[type_FK] = @type_FK,
	[version_number] = @version_number,
	[version_label] = @version_label,
	[document_number] = @document_number,
	[person_FK] = @person_FK,
	[regulatory_status] = @regulatory_status,
	[change_date_from] = @change_date_from,
	[change_date_to] = @change_date_to,
	[effective_start_date_from] = @effective_start_date_from,
	[effective_start_date_to] = @effective_start_date_to,
	[effective_end_date_from] = @effective_end_date_from,
	[effective_end_date_to] = @effective_end_date_to,
	[displayName] = @displayName,
	[user_FK1] = @user_FK1,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic,
	[pp_FK] = @pp_FK,
	[version_date_from] = @version_date_from,
	[version_date_to] = @version_date_to,
	[ev_code] = @ev_code,
	[content] = @content, 
	[language_code] = @language_code, 
	[comments] = @comments
	WHERE [document_saved_search_PK] = @document_saved_search_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[DOCUMENT_SAVED_SEARCH]
		([product_FK], [ap_FK], [project_FK], [activity_FK], [task_FK], [name], [type_FK], [version_number], [version_label], [document_number], [person_FK], [regulatory_status], [change_date_from], [change_date_to], [effective_start_date_from], [effective_start_date_to], [effective_end_date_from], [effective_end_date_to], [displayName], [user_FK1], [gridLayout], [isPublic], [pp_FK], [version_date_from], [version_date_to], [ev_code], [content], [language_code], [comments])
		VALUES
		(@product_FK, @ap_FK, @project_FK, @activity_FK, @task_FK, @name, @type_FK, @version_number, @version_label, @document_number, @person_FK, @regulatory_status, @change_date_from, @change_date_to, @effective_start_date_from, @effective_start_date_to, @effective_end_date_from, @effective_end_date_to, @displayName, @user_FK1, @gridLayout, @isPublic, @pp_FK, @version_date_from, @version_date_to, @ev_code, @content, @language_code, @comments)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
