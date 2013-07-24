-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PERSON_ROLE_GetEntity]
	@person_role_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[person_role_PK], [person_name]
	FROM [dbo].[PERSON_ROLE]
	WHERE [person_role_PK] = @person_role_PK
END
