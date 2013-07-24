-- GetPersonByUserID
CREATE PROCEDURE  [dbo].[proc_USER_GetUserByPersonID]
	@person_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT u.* 
	FROM [USER] u
	LEFT JOIN PERSON p ON p.person_PK = u.person_FK 
	WHERE p.person_PK = @person_PK 
END
