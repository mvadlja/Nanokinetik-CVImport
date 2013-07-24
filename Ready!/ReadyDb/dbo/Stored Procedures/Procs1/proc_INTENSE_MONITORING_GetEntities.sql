-- GetEntities
CREATE PROCEDURE  [dbo].[proc_INTENSE_MONITORING_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[intense_monitoring_PK], [name]
	FROM [dbo].[INTENSE_MONITORING]
END
