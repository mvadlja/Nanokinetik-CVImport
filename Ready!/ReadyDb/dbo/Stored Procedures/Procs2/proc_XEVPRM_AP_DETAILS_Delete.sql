-- Delete
CREATE PROCEDURE  [dbo].[proc_XEVPRM_AP_DETAILS_Delete]
	@xevprm_ap_details_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[XEVPRM_AP_DETAILS] WHERE [xevprm_ap_details_PK] = @xevprm_ap_details_PK
END
