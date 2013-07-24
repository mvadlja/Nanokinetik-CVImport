-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_SERVICE_LOG_GetReportDataSet]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'service_log_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		log.[service_log_PK], log.[description], log.[log_time], person.name+'' ''+person.familyName as responsible_user
		FROM [dbo].[SERVICE_LOG] log
		LEFT JOIN [dbo].[USER]  usr ON usr.user_PK = log.user_FK
		LEFT JOIN [dbo].[PERSON]  person ON person.person_PK = usr.user_PK
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SENT_MESSAGE]
END
