
create PROCEDURE  [dbo].[proc_PERSON_ROLE_GetAvailableEntitiesByPerson]
	@personPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT r.*
	FROM dbo.PERSON_ROLE r
	WHERE r.person_role_PK NOT IN 
	(
		SELECT prMn.person_role_FK
		FROM [dbo].[PERSON_IN_ROLE] prMn
		WHERE prMn.person_FK = @personPk AND @personPk IS NOT NULL
	)
END