-- Save
CREATE PROCEDURE  [dbo].[proc_PERSON_ROLE_Save]
	@person_role_PK int = NULL,
	@person_name nvarchar(25) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PERSON_ROLE]
	SET
	[person_name] = @person_name
	WHERE [person_role_PK] = @person_role_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PERSON_ROLE]
		([person_name])
		VALUES
		(@person_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
