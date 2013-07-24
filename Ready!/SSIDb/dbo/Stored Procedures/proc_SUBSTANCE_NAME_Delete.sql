
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_Delete]
	@substance_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_NAME] WHERE [substance_name_PK] = @substance_name_PK
END
