
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CODE_Save]
	@substance_code_PK int = NULL,
	@code varchar(500) = NULL,
	@code_system_FK int = NULL,
	@code_system_id_FK int = NULL,
	@code_system_status_FK int = NULL,
	@code_system_changedate varchar(10) = NULL,
	@comment varchar(4000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_CODE]
	SET
	[code] = @code,
	[code_system_FK] = @code_system_FK,
	[code_system_id_FK] = @code_system_id_FK,
	[code_system_status_FK] = @code_system_status_FK,
	[code_system_changedate] = @code_system_changedate,
	[comment] = @comment
	WHERE [substance_code_PK] = @substance_code_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_CODE]
		([code], [code_system_FK], [code_system_id_FK], [code_system_status_FK], [code_system_changedate], [comment])
		VALUES
		(@code, @code_system_FK, @code_system_id_FK, @code_system_status_FK, @code_system_changedate, @comment)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
