-- Save
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_Save]
	@document_PK int = NULL,
	@person_FK int = NULL,
	@type_FK int = NULL,
	@name nvarchar(2000) = NULL,
	@description nvarchar(MAX) = NULL,
	@comment nvarchar(MAX) = NULL,
	@document_code nvarchar(250) = NULL,
	@regulatory_status int = NULL,
	@version_number int = NULL,
	@version_label int = NULL,
	@change_date date = NULL,
	@effective_start_date date = NULL,
	@effective_end_date date = NULL,
	@version_date date = NULL,
	@localnumber nvarchar(50) = NULL,
	@version_date_format nvarchar(8) = NULL,
	@attachment_name nvarchar(2000) = NULL,
	@attachment_type_FK int = NULL,
	@EDMSDocumentId nvarchar(128) = NULL,
    @EDMSBindingRule nvarchar(128) = NULL,
    @EDMSModifyDate datetime= NULL,
    @EDMSVersionNumber nvarchar(128) = NULL,
    @EDMSDocument bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[DOCUMENT]
	SET
	[person_FK] = @person_FK,
	[type_FK] = @type_FK,
	[name] = @name,
	[description] = @description,
	[comment] = @comment,
	[document_code] = @document_code,
	[regulatory_status] = @regulatory_status,
	[version_number] = @version_number,
	[version_label] = @version_label,
	[change_date] = @change_date,
	[effective_start_date] = @effective_start_date,
	[effective_end_date] = @effective_end_date,
	[version_date] = @version_date,
	[localnumber] = @localnumber,
	[version_date_format] = @version_date_format,
	[attachment_name] = @attachment_name,
	[attachment_type_FK] = @attachment_type_FK,
	[EDMSDocumentId] = @EDMSDocumentId,
	[EDMSBindingRule] = @EDMSBindingRule,
	[EDMSModifyDate] = @EDMSModifyDate,
	[EDMSVersionNumber] = @EDMSVersionNumber,
	[EDMSDocument] = @EDMSDocument
	WHERE [document_PK] = @document_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[DOCUMENT]
		([person_FK], [type_FK], [name], [description], [comment], [document_code], [regulatory_status], [version_number], [version_label], [change_date], [effective_start_date], [effective_end_date],[version_date], [localnumber], [version_date_format], [attachment_name],[attachment_type_FK], [EDMSBindingRule], [EDMSModifyDate], [EDMSDocumentId], [EDMSVersionNumber], [EDMSDocument])
		VALUES
		(@person_FK, @type_FK, @name, @description, @comment, @document_code, @regulatory_status, @version_number, @version_label, @change_date, @effective_start_date, @effective_end_date, @version_date, @localnumber, @version_date_format, @attachment_name, @attachment_type_FK, @EDMSBindingRule, @EDMSModifyDate, @EDMSDocumentId, @EDMSVersionNumber, @EDMSDocument)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
