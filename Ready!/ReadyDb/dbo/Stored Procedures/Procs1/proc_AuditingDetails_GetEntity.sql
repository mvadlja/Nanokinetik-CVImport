-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_GetEntity]
	@IDAuditingDetail int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[IDAuditingDetail], [MasterID], [ColumnName], [OldValue], [NewValue], [PKValue]
	FROM [dbo].[AuditingDetails]
	WHERE [IDAuditingDetail] = @IDAuditingDetail
END
