-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PERSON_GetAssignedEntitiesByUserRole]
	@UserRolePk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM [dbo].[USER] u
	JOIN dbo.USER_IN_ROLE ur ON ur.user_FK = u.user_PK
	JOIN dbo.PERSON p ON p.person_PK = u.person_FK
	WHERE ur.user_role_FK = @UserRolePk AND @UserRolePk IS NOT NULL

END