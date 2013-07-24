-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_ADMINISTRATION_ROUTE_Delete]
	@adminroute_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_ADMINISTRATION_ROUTE] WHERE [adminroute_PK] = @adminroute_PK
END
