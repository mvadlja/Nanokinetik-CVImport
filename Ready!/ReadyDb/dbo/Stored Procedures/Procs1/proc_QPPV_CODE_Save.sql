-- Save
CREATE PROCEDURE  [dbo].[proc_QPPV_CODE_Save]
	@qppv_code_PK int = NULL,
	@person_FK int = NULL,
	@qppv_code nvarchar(50) = ''
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[QPPV_CODE]
	SET
	person_FK = @person_FK,
	qppv_code = @qppv_code 
	WHERE qppv_code_PK = @qppv_code_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[QPPV_CODE]
		([person_FK], [qppv_code])
		VALUES
		(@person_FK,@qppv_code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
