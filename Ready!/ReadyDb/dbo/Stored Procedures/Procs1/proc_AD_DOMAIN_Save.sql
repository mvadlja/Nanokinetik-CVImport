-- Save
CREATE PROCEDURE  [dbo].[proc_AD_DOMAIN_Save]
	@ad_domain_PK int = NULL,
	@domain_alias nvarchar(100) = NULL,
	@domain_connection_string nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AD_DOMAIN]
	SET
	[domain_alias] = @domain_alias,
	[domain_connection_string] = @domain_connection_string
	WHERE [ad_domain_PK] = @ad_domain_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AD_DOMAIN]
		([domain_alias], [domain_connection_string])
		VALUES
		(@domain_alias, @domain_connection_string)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
