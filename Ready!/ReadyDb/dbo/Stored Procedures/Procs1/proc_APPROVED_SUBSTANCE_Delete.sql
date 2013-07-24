-- Delete
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBSTANCE_Delete]
	@approved_substance_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[APPROVED_SUBSTANCE] WHERE [approved_substance_PK] = @approved_substance_PK
END
