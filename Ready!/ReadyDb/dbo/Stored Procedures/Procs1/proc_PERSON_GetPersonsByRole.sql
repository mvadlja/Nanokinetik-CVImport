-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PERSON_GetPersonsByRole]
	@role_name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM [dbo].[PERSON_IN_ROLE]
	LEFT JOIN [dbo].[PERSON] p ON p.person_PK = [dbo].[PERSON_IN_ROLE].person_FK
	LEFT JOIN [dbo].[PERSON_ROLE] r ON r.person_role_PK = [dbo].[PERSON_IN_ROLE].person_role_FK
	WHERE (r.person_name = @role_name OR @role_name IS NULL)

END
