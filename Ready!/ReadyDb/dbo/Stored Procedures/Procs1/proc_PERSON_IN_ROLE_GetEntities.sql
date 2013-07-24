-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[person_in_role_PK], [person_FK], [person_role_FK]
	FROM [dbo].[PERSON_IN_ROLE]
END
