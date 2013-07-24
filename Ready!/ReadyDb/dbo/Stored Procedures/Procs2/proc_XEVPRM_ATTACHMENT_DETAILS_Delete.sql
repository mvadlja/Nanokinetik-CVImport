-- Delete
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ATTACHMENT_DETAILS_Delete]
	@xevprm_attachment_details_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[XEVPRM_ATTACHMENT_DETAILS] WHERE [xevprm_attachment_details_PK] = @xevprm_attachment_details_PK
END
