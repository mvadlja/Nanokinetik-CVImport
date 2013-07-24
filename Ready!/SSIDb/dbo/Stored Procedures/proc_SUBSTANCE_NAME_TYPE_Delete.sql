
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_TYPE_Delete]
	@substance_name_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_NAME_TYPE] WHERE [substance_name_type_PK] = @substance_name_type_PK
END
