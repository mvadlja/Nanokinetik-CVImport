-- Save
create PROCEDURE [dbo].[proc_MA_SERVICE_LOG_Save]
	@ma_service_log_PK int = NULL,
	@log_time datetime = NULL,
	@description nvarchar(MAX) = NULL,
	@ready_id_FK varchar(16) = NULL,
	@event_type_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MA_SERVICE_LOG]
	SET
	[log_time] = @log_time,
	[description] = @description,
	[ready_id_FK] = @ready_id_FK,
	[event_type_FK] = @event_type_FK
	WHERE [ma_service_log_PK] = @ma_service_log_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MA_SERVICE_LOG]
		([log_time], [description], [ready_id_FK], [event_type_FK])
		VALUES
		(@log_time, @description, @ready_id_FK, @event_type_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
