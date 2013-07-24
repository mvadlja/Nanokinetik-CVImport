
-- Delete
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_TYPE_Delete]
	@official_name_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[OFFICIAL_NAME_TYPE] WHERE [official_name_type_PK] = @official_name_type_PK
END
