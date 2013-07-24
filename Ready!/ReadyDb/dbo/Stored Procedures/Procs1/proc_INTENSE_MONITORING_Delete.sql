-- Delete
CREATE PROCEDURE  [dbo].[proc_INTENSE_MONITORING_Delete]
	@intense_monitoring_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[INTENSE_MONITORING] WHERE [intense_monitoring_PK] = @intense_monitoring_PK
END
