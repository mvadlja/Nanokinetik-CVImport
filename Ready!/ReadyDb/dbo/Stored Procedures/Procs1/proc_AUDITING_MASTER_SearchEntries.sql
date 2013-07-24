-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_SearchEntries]
	@PageNum [int] = 1,
	@PageSize [int] = 20,
	@username [nvarchar](100) = NULL,
	@server_name [nvarchar](100) = NULL,
	@db_name [nvarchar](200) = NULL,
	@table_name [nvarchar](200) = NULL,
	@DateFrom [datetime] = NULL,
	@DateTill [datetime] = NULL,
	@columns [nvarchar](MAX) = NULL,
	@operations [nvarchar](128) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY auditing_master_PK) AS RowNum,
		am.* FROM 
 		(
 			SELECT DISTINCT 
 			AUDITING_MASTER.* 
 			FROM  
 			AUDITING_MASTER
			LEFT JOIN AUDITING_DETAILS ON AUDITING_DETAILS.master_ID = AUDITING_MASTER.auditing_master_PK
			WHERE (AUDITING_MASTER.[db_name] LIKE '%' + @db_name + '%' OR @db_name IS NULL)
			AND (AUDITING_MASTER.table_name LIKE '%' + @table_name + '%' OR @table_name IS NULL)
			AND (AUDITING_MASTER.operation COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@operations)) OR @operations IS NULL)
			AND (AUDITING_DETAILS.column_name COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@columns)) OR @columns IS NULL)
			AND (AUDITING_MASTER.username LIKE '%' + @username + '%' OR @username IS NULL)
			AND (AUDITING_MASTER.server_name LIKE '%' + @server_name + '%' OR @server_name IS NULL)
			AND (AUDITING_MASTER.[date] BETWEEN convert(datetime, ISNULL(@DateFrom, Date), 104) AND dateadd(day, 1, convert(datetime, ISNULL(@DateTill, Date), 104)))
		) am
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(DISTINCT auditing_master_PK) FROM AUDITING_MASTER
	LEFT JOIN AUDITING_DETAILS ON AUDITING_DETAILS.master_ID = AUDITING_MASTER.auditing_master_PK
	WHERE (AUDITING_MASTER.[db_name] LIKE '%' + @db_name + '%' OR @db_name IS NULL)
	AND (AUDITING_MASTER.table_name LIKE '%' + @table_name + '%' OR @table_name IS NULL)
	AND (AUDITING_MASTER.operation COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@operations)) OR @operations IS NULL)
	AND (AUDITING_DETAILS.column_name COLLATE DATABASE_DEFAULT IN (SELECT * FROM dbo.ParseDelimitedString(',',@columns)) OR @columns IS NULL)
	AND (AUDITING_MASTER.username LIKE '%' + @username + '%' OR @username IS NULL)
	AND (AUDITING_MASTER.server_name LIKE '%' + @server_name + '%' OR @server_name IS NULL)
	AND (AUDITING_MASTER.[date] BETWEEN convert(datetime, ISNULL(@DateFrom, Date), 104) AND dateadd(day, 1, convert(datetime, ISNULL(@DateTill, Date), 104)))
END
