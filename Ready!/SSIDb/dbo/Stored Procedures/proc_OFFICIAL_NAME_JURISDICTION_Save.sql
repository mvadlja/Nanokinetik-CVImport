
-- Save
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_JURISDICTION_Save]
	@jurisdiction_PK int = NULL,
	@on_jurisd varchar(3) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[OFFICIAL_NAME_JURISDICTION]
	SET
	[on_jurisd] = @on_jurisd
	WHERE [jurisdiction_PK] = @jurisdiction_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[OFFICIAL_NAME_JURISDICTION]
		([on_jurisd])
		VALUES
		(@on_jurisd)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
