-- Save
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ATTACHMENT_DETAILS_Save]
	@xevprm_attachment_details_PK int = NULL,
	@attachment_FK int = NULL,
	@file_name nvarchar(200) = NULL,
	@file_type nvarchar(10) = NULL,
	@attachment_name nvarchar(2000) = NULL,
	@attachment_type nvarchar(10) = NULL,
	@language_code nvarchar(50) = NULL,
	@attachment_version nvarchar(10) = NULL,
	@attachment_version_date datetime = NULL,
	@operation_type int = NULL,
	@ev_code nvarchar(60) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[XEVPRM_ATTACHMENT_DETAILS]
	SET
	[attachment_FK] = @attachment_FK,
	[file_name] = @file_name,
	[file_type] = @file_type,
	[attachment_name] = @attachment_name,
	[attachment_type] = @attachment_type,
	[language_code] = @language_code,
	[attachment_version] = @attachment_version,
	[attachment_version_date] = @attachment_version_date,
	[operation_type] = @operation_type,
	[ev_code] = @ev_code
	WHERE [xevprm_attachment_details_PK] = @xevprm_attachment_details_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[XEVPRM_ATTACHMENT_DETAILS]
		([attachment_FK], [file_name], [file_type], [attachment_name], [attachment_type], [language_code], [attachment_version], [attachment_version_date], [operation_type], [ev_code])
		VALUES
		(@attachment_FK, @file_name, @file_type, @attachment_name, @attachment_type, @language_code, @attachment_version, @attachment_version_date, @operation_type, @ev_code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
