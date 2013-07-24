-- Save
CREATE PROCEDURE  [dbo].[proc_AS_PREVIOUS_EV_CODE_Save]
	@as_previous_ev_code_PK int = NULL,
	@devevcode nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AS_PREVIOUS_EV_CODE]
	SET
	[devevcode] = @devevcode
	WHERE [as_previous_ev_code_PK] = @as_previous_ev_code_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AS_PREVIOUS_EV_CODE]
		([devevcode])
		VALUES
		(@devevcode)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
