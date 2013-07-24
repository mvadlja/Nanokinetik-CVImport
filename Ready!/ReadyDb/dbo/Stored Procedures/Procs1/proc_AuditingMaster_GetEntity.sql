-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_GetEntity]
	@IDAuditingMaster int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDAuditingMaster], [Username], [DBName], [TableName], [Date], [Operation], [ServerName]
	FROM [dbo].[AuditingMaster]
	WHERE [IDAuditingMaster] = @IDAuditingMaster
END
