-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_TYPE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_type_PK], [activity_FK], [type_FK]
	FROM [dbo].[ACTIVITY_TYPE_MN]
END
