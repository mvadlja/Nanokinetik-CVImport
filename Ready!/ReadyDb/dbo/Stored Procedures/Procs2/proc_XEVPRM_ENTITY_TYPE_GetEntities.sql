-- GetEntities
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ENTITY_TYPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_entity_type_PK], [name], [table_name]
	FROM [dbo].[XEVPRM_ENTITY_TYPE]
END
