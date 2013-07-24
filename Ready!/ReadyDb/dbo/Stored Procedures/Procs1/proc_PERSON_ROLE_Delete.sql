-- Delete
CREATE PROCEDURE  [dbo].[proc_PERSON_ROLE_Delete]
	@person_role_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PERSON_ROLE] WHERE [person_role_PK] = @person_role_PK
END
