-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDAuditingDetail], [MasterID], [ColumnName], [OldValue], [NewValue], [PKValue]
	FROM [dbo].[AuditingDetails]
END
