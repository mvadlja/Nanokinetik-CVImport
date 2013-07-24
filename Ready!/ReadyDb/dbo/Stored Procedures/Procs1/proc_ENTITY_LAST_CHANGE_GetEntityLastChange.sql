-- GetActivitiesByDocument
CREATE PROCEDURE  [dbo].[proc_ENTITY_LAST_CHANGE_GetEntityLastChange]
	@table_name varchar(MAX) = NULL,
	@entity_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT lc.*
	FROM [dbo].[ENTITY_LAST_CHANGE] lc
	WHERE (lc.change_table LIKE @table_name AND lc.entity_FK = @entity_PK)

END
