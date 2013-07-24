
-- Save
CREATE PROCEDURE [dbo].[proc_SN_ON_MN_Save]
	@sn_on_mn_PK int = NULL,
	@official_name_FK int = NULL,
	@substance_name_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SN_ON_MN]
	SET
	[official_name_FK] = @official_name_FK,
	[substance_name_FK] = @substance_name_FK
	WHERE [sn_on_mn_PK] = @sn_on_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SN_ON_MN]
		([official_name_FK], [substance_name_FK])
		VALUES
		(@official_name_FK, @substance_name_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
