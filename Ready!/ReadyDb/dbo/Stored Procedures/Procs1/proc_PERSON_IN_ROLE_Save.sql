-- Save
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_Save]
	@person_in_role_PK int = NULL,
	@person_FK int = NULL,
	@person_role_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PERSON_IN_ROLE]
	SET
	[person_FK] = @person_FK,
	[person_role_FK] = @person_role_FK
	WHERE [person_in_role_PK] = @person_in_role_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PERSON_IN_ROLE]
		([person_FK], [person_role_FK])
		VALUES
		(@person_FK, @person_role_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
