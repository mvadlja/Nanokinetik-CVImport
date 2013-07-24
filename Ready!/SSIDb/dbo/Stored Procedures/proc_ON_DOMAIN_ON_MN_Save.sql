
-- Save
CREATE PROCEDURE [dbo].[proc_ON_DOMAIN_ON_MN_Save]
	@on_domain_on_mn_PK int = NULL,
	@on_domain_FK int = NULL,
	@on_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ON_DOMAIN_ON_MN]
	SET
	[on_domain_FK] = @on_domain_FK,
	[on_FK] = @on_FK
	WHERE [on_domain_on_mn_PK] = @on_domain_on_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ON_DOMAIN_ON_MN]
		([on_domain_FK], [on_FK])
		VALUES
		(@on_domain_FK, @on_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
