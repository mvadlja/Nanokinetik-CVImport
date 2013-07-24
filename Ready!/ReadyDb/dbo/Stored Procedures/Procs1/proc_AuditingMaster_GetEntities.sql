-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDAuditingMaster], [Username], [DBName], [TableName], [Date], [Operation], [ServerName]
	FROM [dbo].[AuditingMaster]
END
