-- Save
CREATE PROCEDURE  [dbo].[proc_DOMAIN_Save]
	@domain_PK int = NULL,
	@name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[DOMAIN]
	SET
	[name] = @name
	WHERE [domain_PK] = @domain_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[DOMAIN]
		([name])
		VALUES
		(@name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
