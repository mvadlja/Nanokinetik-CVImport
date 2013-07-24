
-- Delete
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_Delete]
	@official_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[OFFICIAL_NAME] WHERE [official_name_PK] = @official_name_PK
END
