-- GetEntity
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ENTITY_TYPE_GetEntity]
	@xevprm_entity_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_entity_type_PK], [name], [table_name]
	FROM [dbo].[XEVPRM_ENTITY_TYPE]
	WHERE [xevprm_entity_type_PK] = @xevprm_entity_type_PK
END
