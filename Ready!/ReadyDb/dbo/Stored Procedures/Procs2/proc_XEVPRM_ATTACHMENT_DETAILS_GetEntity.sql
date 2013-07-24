-- GetEntity
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ATTACHMENT_DETAILS_GetEntity]
	@xevprm_attachment_details_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_attachment_details_PK], [attachment_FK], [file_name], [file_type], [attachment_name], [attachment_type], [language_code], [attachment_version], [attachment_version_date], [operation_type], [ev_code]
	FROM [dbo].[XEVPRM_ATTACHMENT_DETAILS]
	WHERE [xevprm_attachment_details_PK] = @xevprm_attachment_details_PK
END
