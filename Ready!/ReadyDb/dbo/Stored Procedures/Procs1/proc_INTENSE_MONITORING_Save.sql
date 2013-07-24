-- Save
CREATE PROCEDURE  [dbo].[proc_INTENSE_MONITORING_Save]
	@intense_monitoring_PK int = NULL,
	@name int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[INTENSE_MONITORING]
	SET
	[name] = @name
	WHERE [intense_monitoring_PK] = @intense_monitoring_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[INTENSE_MONITORING]
		([name])
		VALUES
		(@name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
