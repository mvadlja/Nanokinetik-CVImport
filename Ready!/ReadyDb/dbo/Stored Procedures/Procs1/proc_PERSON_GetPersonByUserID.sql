-- GetPersonByUserID
CREATE PROCEDURE  [dbo].[proc_PERSON_GetPersonByUserID]
	@user_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM [USER] u
	LEFT JOIN PERSON p ON p.person_PK = u.person_FK 
	WHERE u.user_PK = @user_PK 
END
