
-- Save
CREATE PROCEDURE [dbo].[proc_RS_SR_MN_Save]
	@rs_sr_mn_PK int = NULL,
	@rs_FK int = NULL,
	@sr_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_SR_MN]
	SET
	[rs_FK] = @rs_FK,
	[sr_FK] = @sr_FK
	WHERE [rs_sr_mn_PK] = @rs_sr_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_SR_MN]
		([rs_FK], [sr_FK])
		VALUES
		(@rs_FK, @sr_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
