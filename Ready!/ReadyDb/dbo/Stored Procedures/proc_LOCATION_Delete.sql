-- Delete
create PROCEDURE proc_LOCATION_Delete
	@location_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[LOCATION] WHERE [location_PK] = @location_PK
END