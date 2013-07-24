-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PERSON_ROLE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[person_role_PK], [person_name]
	FROM [dbo].[PERSON_ROLE]
END
