-- GetTypesByActivity
CREATE PROCEDURE  proc_ACTIVITY_TYPE_MN_GetTypesByActivity
	@activity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.activity_type_PK, a.activity_PK, t.type_PK, t.name
	FROM [dbo].[ACTIVITY_TYPE_MN] mn
	LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = mn.activity_FK
	LEFT JOIN [dbo].[TYPE] t ON t.type_PK = mn.type_FK
	WHERE (mn.activity_FK = @activity_FK OR @activity_FK IS NULL)

END
