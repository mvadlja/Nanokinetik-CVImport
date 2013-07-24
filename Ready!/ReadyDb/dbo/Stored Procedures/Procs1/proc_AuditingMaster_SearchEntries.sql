-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_SearchEntries]
	@PageNum [int] = 1,
	@PageSize [int] = 20,
	@Username [nvarchar](100) = NULL,
	@ServerName [nvarchar](100) = NULL,
	@DBName [nvarchar](200) = NULL,
	@TableName [nvarchar](200) = NULL,
	@DateFrom [datetime] = NULL,
	@DateTill [datetime] = NULL,
	@Columns [nvarchar](MAX) = NULL,
	@Operations [nvarchar](128) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY IDAuditingMaster) AS RowNum,
		am.* FROM 
 		(
 			SELECT DISTINCT 
 			AuditingMaster.* 
 			FROM  
 			AuditingMaster
			LEFT JOIN AuditingDetails ON AuditingDetails.MasterID = AuditingMaster.IDAuditingMaster
			WHERE (AuditingMaster.DBName LIKE '%' + @DBName + '%' OR @DBName IS NULL)
			AND (AuditingMaster.TableName LIKE '%' + @TableName + '%' OR @TableName IS NULL)
			AND (AuditingMaster.Operation COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@Operations)) OR @Operations IS NULL)
			AND (AuditingDetails.ColumnName COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@Columns)) OR @Columns IS NULL)
			AND (AuditingMaster.Username LIKE '%' + @Username + '%' OR @Username IS NULL)
			AND (AuditingMaster.ServerName LIKE '%' + @ServerName + '%' OR @ServerName IS NULL)
			AND (AuditingMaster.Date BETWEEN convert(datetime, ISNULL(@DateFrom, Date), 104) AND dateadd(day, 1, convert(datetime, ISNULL(@DateTill, Date), 104)))
		) am
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(DISTINCT IDAuditingMaster) FROM AuditingMaster
	LEFT JOIN AuditingDetails ON AuditingDetails.MasterID = AuditingMaster.IDAuditingMaster
	WHERE (AuditingMaster.DBName LIKE '%' + @DBName + '%' OR @DBName IS NULL)
	AND (AuditingMaster.TableName LIKE '%' + @TableName + '%' OR @TableName IS NULL)
	AND (AuditingMaster.Operation COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@Operations)) OR @Operations IS NULL)
	AND (AuditingDetails.ColumnName COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@Columns)) OR @Columns IS NULL)
	AND (AuditingMaster.Username LIKE '%' + @Username + '%' OR @Username IS NULL)
	AND (AuditingMaster.ServerName LIKE '%' + @ServerName + '%' OR @ServerName IS NULL)
	AND (AuditingMaster.Date BETWEEN convert(datetime, ISNULL(@DateFrom, Date), 104) AND dateadd(day, 1, convert(datetime, ISNULL(@DateTill, Date), 104)))
END
