-- GetEntity
create PROCEDURE [dbo].[proc_NOTIFICATION_TYPE_GetEntity]
	@notification_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[notification_type_PK], [name]
	FROM [dbo].[NOTIFICATION_TYPE]
	WHERE [notification_type_PK] = @notification_type_PK
END
