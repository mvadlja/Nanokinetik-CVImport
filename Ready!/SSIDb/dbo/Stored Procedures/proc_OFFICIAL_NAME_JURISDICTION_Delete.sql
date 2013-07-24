
-- Delete
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_JURISDICTION_Delete]
	@jurisdiction_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[OFFICIAL_NAME_JURISDICTION] WHERE [jurisdiction_PK] = @jurisdiction_PK
END
