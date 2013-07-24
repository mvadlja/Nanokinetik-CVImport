-- Save
CREATE PROCEDURE proc_XEVPRM_ENTITY_DETAILS_MN_Save
	@xevprm_entity_details_mn_PK int = NULL,
	@xevprm_message_FK int = NULL,
	@xevprm_entity_details_FK int = NULL,
	@xevprm_entity_type_FK int = NULL,
	@xevprm_entity_FK int = NULL,
	@xevprm_operation_type int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[XEVPRM_ENTITY_DETAILS_MN]
	SET
	[xevprm_message_FK] = @xevprm_message_FK,
	[xevprm_entity_details_FK] = @xevprm_entity_details_FK,
	[xevprm_entity_type_FK] = @xevprm_entity_type_FK,
	[xevprm_entity_FK] = @xevprm_entity_FK,
	[xevprm_operation_type] = @xevprm_operation_type
	WHERE [xevprm_entity_details_mn_PK] = @xevprm_entity_details_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[XEVPRM_ENTITY_DETAILS_MN]
		([xevprm_message_FK], [xevprm_entity_details_FK], [xevprm_entity_type_FK], [xevprm_entity_FK], [xevprm_operation_type])
		VALUES
		(@xevprm_message_FK, @xevprm_entity_details_FK, @xevprm_entity_type_FK, @xevprm_entity_FK, @xevprm_operation_type)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
