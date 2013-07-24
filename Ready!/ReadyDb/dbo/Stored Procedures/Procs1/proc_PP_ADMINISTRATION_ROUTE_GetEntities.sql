-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PP_ADMINISTRATION_ROUTE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[adminroute_PK], [adminroutecode], [resolutionmode], [ev_code]
	FROM [dbo].[PP_ADMINISTRATION_ROUTE]
END
