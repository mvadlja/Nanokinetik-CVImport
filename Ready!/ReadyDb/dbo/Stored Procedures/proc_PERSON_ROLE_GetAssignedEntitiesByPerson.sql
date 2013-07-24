
create PROCEDURE  [dbo].[proc_PERSON_ROLE_GetAssignedEntitiesByPerson]
	@personPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT r.*
	FROM [dbo].[PERSON_IN_ROLE] prMn
	JOIN dbo.PERSON_ROLE r ON r.person_role_PK = prMn.person_role_FK
	WHERE prMn.person_FK = @personPk AND @personPk IS NOT NULL
END