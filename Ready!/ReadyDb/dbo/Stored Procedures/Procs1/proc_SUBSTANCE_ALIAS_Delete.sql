-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ALIAS_Delete]
	@substance_alias_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_ALIAS] WHERE [substance_alias_PK] = @substance_alias_PK
END
