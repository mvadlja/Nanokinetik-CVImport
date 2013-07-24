-- Save
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_FORM_Save]
	@pharmaceutical_form_PK int = NULL,
	@name nvarchar(200) = NULL,
	@ev_code nvarchar(60) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PHARMACEUTICAL_FORM]
	SET
	[name] = @name,
	[ev_code] = @ev_code
	WHERE [pharmaceutical_form_PK] = @pharmaceutical_form_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PHARMACEUTICAL_FORM]
		([name], [ev_code])
		VALUES
		(@name, @ev_code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
