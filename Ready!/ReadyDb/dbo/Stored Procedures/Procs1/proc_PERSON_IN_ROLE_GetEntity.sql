-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_GetEntity]
	@person_in_role_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[person_in_role_PK], [person_FK], [person_role_FK]
	FROM [dbo].[PERSON_IN_ROLE]
	WHERE [person_in_role_PK] = @person_in_role_PK
END
