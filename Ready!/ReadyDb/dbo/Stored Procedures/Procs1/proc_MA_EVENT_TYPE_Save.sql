-- Save
create PROCEDURE [dbo].[proc_MA_EVENT_TYPE_Save]
	@ma_event_type_PK int = NULL,
	@name nvarchar(200) = NULL,
	@enum_name nvarchar(50) = NULL,
	@ma_event_type_severity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MA_EVENT_TYPE]
	SET
	[name] = @name,
	[enum_name] = @enum_name,
	[ma_event_type_severity_FK] = @ma_event_type_severity_FK
	WHERE [ma_event_type_PK] = @ma_event_type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MA_EVENT_TYPE]
		([name], [enum_name], [ma_event_type_severity_FK])
		VALUES
		(@name, @enum_name, @ma_event_type_severity_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
