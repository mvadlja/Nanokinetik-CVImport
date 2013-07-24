-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PP_ADMINISTRATION_ROUTE_GetEntity]
	@adminroute_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[adminroute_PK], [adminroutecode], [resolutionmode], [ev_code]
	FROM [dbo].[PP_ADMINISTRATION_ROUTE]
	WHERE [adminroute_PK] = @adminroute_PK
END
