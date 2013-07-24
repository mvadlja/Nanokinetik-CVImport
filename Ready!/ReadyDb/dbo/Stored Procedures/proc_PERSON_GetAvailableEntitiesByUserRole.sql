-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PERSON_GetAvailableEntitiesByUserRole]
	@UserRolePk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM dbo.[PERSON] p
	WHERE p.person_PK in (SELECT person_FK FROM dbo.[USER]) AND p.person_PK NOT IN 
	( 
		SELECT u.person_FK 
		FROM [dbo].[USER] u
		JOIN dbo.USER_IN_ROLE ur ON ur.user_FK = u.user_PK
		WHERE ur.user_role_FK = @UserRolePk AND @UserRolePk IS NOT NULL
	)

END