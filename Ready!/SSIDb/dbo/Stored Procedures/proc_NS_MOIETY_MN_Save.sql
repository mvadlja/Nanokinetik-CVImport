
-- Save
CREATE PROCEDURE [dbo].[proc_NS_MOIETY_MN_Save]
	@ns_moiety_mn_PK int = NULL,
	@moiety_FK int = NULL,
	@ns_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[NS_MOIETY_MN]
	SET
	[moiety_FK] = @moiety_FK,
	[ns_FK] = @ns_FK
	WHERE [ns_moiety_mn_PK] = @ns_moiety_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[NS_MOIETY_MN]
		([moiety_FK], [ns_FK])
		VALUES
		(@moiety_FK, @ns_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
