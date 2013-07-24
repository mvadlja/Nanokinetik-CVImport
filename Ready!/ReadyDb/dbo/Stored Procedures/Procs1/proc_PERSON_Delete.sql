-- Delete
CREATE PROCEDURE  [dbo].[proc_PERSON_Delete]
	@person_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PERSON] WHERE [person_PK] = @person_PK
END
