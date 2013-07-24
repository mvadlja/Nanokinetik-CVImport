-- GetPersonRolePKsByPersonPK
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_GetPersonRolePKsByPersonPK]
	@person_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[PERSON_IN_ROLE].[person_in_role_PK], [dbo].[PERSON_IN_ROLE].[person_FK], [dbo].[PERSON_IN_ROLE].[person_role_FK]
	FROM [dbo].[PERSON_IN_ROLE]
	WHERE ([dbo].[PERSON_IN_ROLE].[person_FK] = @person_FK OR @person_FK IS NULL)

END
