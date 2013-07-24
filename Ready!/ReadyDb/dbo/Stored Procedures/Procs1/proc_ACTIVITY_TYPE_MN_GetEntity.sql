-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_TYPE_MN_GetEntity]
	@activity_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_type_PK], [activity_FK], [type_FK]
	FROM [dbo].[ACTIVITY_TYPE_MN]
	WHERE [activity_type_PK] = @activity_type_PK
END
