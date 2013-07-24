
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBTYPE_Delete]
	@subtype_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBTYPE] WHERE [subtype_PK] = @subtype_PK
END
