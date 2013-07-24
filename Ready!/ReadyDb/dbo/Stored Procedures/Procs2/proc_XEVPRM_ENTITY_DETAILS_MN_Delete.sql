-- Delete
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ENTITY_DETAILS_MN_Delete]
	@xevprm_entity_details_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[XEVPRM_ENTITY_DETAILS_MN] WHERE [xevprm_entity_details_mn_PK] = @xevprm_entity_details_mn_PK
END
