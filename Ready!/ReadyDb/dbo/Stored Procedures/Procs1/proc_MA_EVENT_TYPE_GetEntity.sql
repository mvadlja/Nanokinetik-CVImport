-- GetEntity
create PROCEDURE [dbo].[proc_MA_EVENT_TYPE_GetEntity]
	@ma_event_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_event_type_PK], [name], [enum_name], [ma_event_type_severity_FK]
	FROM [dbo].[MA_EVENT_TYPE]
	WHERE [ma_event_type_PK] = @ma_event_type_PK
END
