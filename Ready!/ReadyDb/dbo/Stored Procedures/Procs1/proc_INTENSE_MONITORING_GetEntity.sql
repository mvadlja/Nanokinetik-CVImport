-- GetEntity
CREATE PROCEDURE  [dbo].[proc_INTENSE_MONITORING_GetEntity]
	@intense_monitoring_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[intense_monitoring_PK], [name]
	FROM [dbo].[INTENSE_MONITORING]
	WHERE [intense_monitoring_PK] = @intense_monitoring_PK
END
