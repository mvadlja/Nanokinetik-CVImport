
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_Delete]
	@substance_s_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE] WHERE [substance_s_PK] = @substance_s_PK
END
