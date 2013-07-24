
-- Save
CREATE PROCEDURE [dbo].[proc_NS_PROPERTY_MN_Save]
	@ns_property_mn_PK int = NULL,
	@ns_FK int = NULL,
	@property_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[NS_PROPERTY_MN]
	SET
	[ns_FK] = @ns_FK,
	[property_FK] = @property_FK
	WHERE [ns_property_mn_PK] = @ns_property_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[NS_PROPERTY_MN]
		([ns_FK], [property_FK])
		VALUES
		(@ns_FK, @property_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
