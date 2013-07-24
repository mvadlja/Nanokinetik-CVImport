-- Delete
CREATE PROCEDURE  [dbo].[proc_TYPE_Delete]
	@type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TYPE] WHERE [type_PK] = @type_PK
END
