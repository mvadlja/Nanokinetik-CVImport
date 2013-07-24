-- Delete
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ENTITY_TYPE_Delete]
	@xevprm_entity_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[XEVPRM_ENTITY_TYPE] WHERE [xevprm_entity_type_PK] = @xevprm_entity_type_PK
END
