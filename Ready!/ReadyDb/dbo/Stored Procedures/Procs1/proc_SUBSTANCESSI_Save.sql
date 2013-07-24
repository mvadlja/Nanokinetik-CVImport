-- Save
CREATE PROCEDURE  [dbo].[proc_SUBSTANCESSI_Save]
	@substancessis_PK int = NULL,
	@valid_according_ssi bit = NULL,
	@ssi_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCESSI]
	SET
	[valid_according_ssi] = @valid_according_ssi,
	[ssi_FK] = @ssi_FK
	WHERE [substancessis_PK] = @substancessis_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCESSI]
		([valid_according_ssi], [ssi_FK])
		VALUES
		(@valid_according_ssi, @ssi_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
