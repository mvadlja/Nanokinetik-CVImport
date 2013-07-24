-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [IDAuditingMaster]) AS RowNum,
		[IDAuditingMaster], [Username], [DBName], [TableName], [Date], [Operation], [ServerName]
		FROM [dbo].[AuditingMaster]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AuditingMaster]
END
