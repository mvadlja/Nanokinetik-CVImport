
-- Save
CREATE PROCEDURE [dbo].[proc_ON_ONJ_MN_Save]
	@on_onj_mn_PK int = NULL,
	@onj_FK int = NULL,
	@on_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ON_ONJ_MN]
	SET
	[onj_FK] = @onj_FK,
	[on_FK] = @on_FK
	WHERE [on_onj_mn_PK] = @on_onj_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ON_ONJ_MN]
		([onj_FK], [on_FK])
		VALUES
		(@onj_FK, @on_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
