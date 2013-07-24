
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_DeleteByPerson]
	@personPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PERSON_IN_ROLE] WHERE person_FK = @personPk AND @personPk IS NOT NULL
END