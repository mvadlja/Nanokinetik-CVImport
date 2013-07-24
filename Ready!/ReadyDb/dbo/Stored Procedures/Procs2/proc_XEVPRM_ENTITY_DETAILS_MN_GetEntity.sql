-- GetEntity
CREATE PROCEDURE proc_XEVPRM_ENTITY_DETAILS_MN_GetEntity
	@xevprm_entity_details_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_entity_details_mn_PK], [xevprm_message_FK], [xevprm_entity_details_FK], [xevprm_entity_type_FK], [xevprm_entity_FK], [xevprm_operation_type]
	FROM [dbo].[XEVPRM_ENTITY_DETAILS_MN]
	WHERE [xevprm_entity_details_mn_PK] = @xevprm_entity_details_mn_PK
END
