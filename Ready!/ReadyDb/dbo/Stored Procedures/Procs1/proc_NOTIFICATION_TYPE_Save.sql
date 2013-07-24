-- Save
create PROCEDURE [dbo].[proc_NOTIFICATION_TYPE_Save]
	@notification_type_PK int = NULL,
	@name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[NOTIFICATION_TYPE]
	SET
	[name] = @name
	WHERE [notification_type_PK] = @notification_type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[NOTIFICATION_TYPE]
		([name])
		VALUES
		(@name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
