-- GetEntity
CREATE PROCEDURE  proc_ENTITY_LAST_CHANGE_GetEntity
	@last_change_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[last_change_PK], [change_table], [change_date], [user_FK], [entity_FK]
	FROM [dbo].[ENTITY_LAST_CHANGE]
	WHERE [last_change_PK] = @last_change_PK
END
