-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [IDAuditingDetail]) AS RowNum,
		[IDAuditingDetail], [MasterID], [ColumnName], [OldValue], [NewValue], [PKValue]
		FROM [dbo].[AuditingDetails]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AuditingDetails]
END
