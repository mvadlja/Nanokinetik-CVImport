
CREATE PROCEDURE [dbo].[proc_AuditingMaster_GetListFormDataSet]
	@Version nvarchar(250) = NULL,
	@ChangeDate nvarchar(250) = NULL,
	@Username nvarchar(250) = NULL,
	
	@QueryBy nvarchar(100) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ChangeDate'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	DECLARE @OriginalOrderByQuery nvarchar(1000) = @OrderByQuery;
	DECLARE @OrderByVersion nvarchar(20) = 'Version ASC';
	DECLARE @Index int = NULL;

	SET @Index = CHARINDEX(@OrderByVersion, @OrderByQuery);

	IF (@Index = 0)
	BEGIN
		SET @OrderByVersion = 'Version DESC'
		SET @Index = CHARINDEX(@OrderByVersion, @OrderByQuery);
	END

	IF (@Index = 0)
		SET @OrderByVersion = NULL;
	ELSE IF (@Index = 1)
	BEGIN
		SET @OrderByQuery = ISNULL(NULLIF(RTRIM(REPLACE(@OrderByQuery, @OrderByVersion + ',', '')),''), 'ChangeDate');
		SET @OrderByQuery = ISNULL(NULLIF(RTRIM(REPLACE(@OrderByQuery, @OrderByVersion, '')),''), 'ChangeDate');
	END
	ELSE IF (@Index > 1)
	BEGIN
		SET @OrderByQuery = ISNULL(NULLIF(RTRIM(REPLACE(', ' + @OrderByQuery, @OrderByVersion, '')),''), 'ChangeDate');
	END
	ELSE
		SET @OrderByVersion = NULL;


	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS Version,
		Audit.* FROM
		(
			SELECT
				MIN(am.IDAuditingMaster) IDAuditingMaster,
				MIN(am.Date) as ChangeDate,
				MIN(am.Username) as Username,
				SessionToken as SessionToken,
				'''' as [Details]
			FROM AuditingMaster am
			INNER JOIN AuditingDetails ad on am.IDAuditingMaster = ad.MasterID
			WHERE am.TableName =  ''' + CONVERT (nvarchar(MAX), @QueryBy) + '''
		      AND ad.PKValue = ' + CONVERT (nvarchar(10), @EntityPk) + '
		      AND ((ad.NewValue IS NOT NULL AND ad.NewValue != '') OR (ad.OldValue IS NOT NULL AND ad.OldValue != ''))
			  AND ColumnName not like ''%_PK''
		'
		SET @TempWhereQuery = '';

		-- @ChangeDate
		IF @ChangeDate IS NOT NULL
		BEGIN
			SET @TempWhereQuery = @TempWhereQuery + ' AND Date LIKE N''%' + REPLACE(REPLACE(@ChangeDate,'[','[[]'),'''','''''') + '%'''
		END
		
	    -- @Username
		IF @Username IS NOT NULL
		BEGIN
			SET @TempWhereQuery = @TempWhereQuery + ' AND Username LIKE N''%' + REPLACE(REPLACE(@Username,'[','[[]'),'''','''''') + '%'''
		END
		
		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + 
	   ' GROUP BY SessionToken) Audit
	),
	  
	FinalResult AS 
	(
		SELECT PagingResult.* 
		FROM PagingResult
		'
		-- @Version
		IF @Version IS NOT NULL
		BEGIN
			SET @Query = @Query + ' WHERE Version LIKE N''%' + REPLACE(REPLACE(@Version,'[','[[]'),'''','''''') + '%'''
		END

		IF @OrderByVersion IS NOT NULL
		BEGIN
			SET @Query = @Query + ' ORDER BY ' + @OriginalOrderByQuery
		END

		SET @Query = @Query + '
	)
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,*
		FROM FinalResult
	)x
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM FinalResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END