-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_GetPersonsByRole]
	@person_role int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[PERSON_IN_ROLE]
	LEFT JOIN [dbo].[PERSON] p ON p.person_PK = [dbo].[PERSON_IN_ROLE].person_FK
	LEFT JOIN [dbo].[PERSON_ROLE] pr ON pr.person_role_PK = [dbo].[PERSON_IN_ROLE].person_role_FK
	WHERE (pr.person_name = @person_role OR @person_role IS NULL)

END
