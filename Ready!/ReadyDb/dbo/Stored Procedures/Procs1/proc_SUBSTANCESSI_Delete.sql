-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBSTANCESSI_Delete]
	@substancessis_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCESSI] WHERE [substancessis_PK] = @substancessis_PK
END
