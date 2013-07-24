
-- Save
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_DOMAIN_Save]
	@on_domain_PK int = NULL,
	@name_domain varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[OFFICIAL_NAME_DOMAIN]
	SET
	[name_domain] = @name_domain
	WHERE [on_domain_PK] = @on_domain_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[OFFICIAL_NAME_DOMAIN]
		([name_domain])
		VALUES
		(@name_domain)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
