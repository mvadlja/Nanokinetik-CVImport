-- GetEntities
create PROCEDURE [dbo].[proc_NOTIFICATION_TYPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[notification_type_PK], [name]
	FROM [dbo].[NOTIFICATION_TYPE]
END
