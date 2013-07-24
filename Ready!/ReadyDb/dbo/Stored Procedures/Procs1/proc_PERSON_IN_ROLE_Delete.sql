-- Delete
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_Delete]
	@person_in_role_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PERSON_IN_ROLE] WHERE [person_in_role_PK] = @person_in_role_PK
END
