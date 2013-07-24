-- ProductPISearcher
CREATE PROCEDURE  [dbo].[proc_ATC_GetATCSearcher]
	@code nvarchar(100) = NULL,
	@name nvarchar(100) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ATCcode'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		DistinctATC.* FROM
		(
			--SELECT DISTINCT
			--[atc_PK] AS ID, [atccode] AS ATCcode, [name] as Name
			--FROM [dbo].[ATC] 
			select DISTINCT f.atc_PK AS ID, f.atccode AS Name, f.[name] as ATCcode
			from (
			   select atccode, min(atc_PK) as atc_PKx
			   from [dbo].[ATC] group by atccode
			) as x inner join [dbo].[ATC] as f on f.atccode = x.atccode and f.atc_PK = x.atc_PKx
			'
			SET @TempWhereQuery = 'where f.atccode is not null and f.atccode != '''' ';


			-- Check nullability for every parameter
			-- @code
			IF @code IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'f.[atccode] LIKE ''%' + REPLACE(@code,'[','[[]') + '%'''
			END
			
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'f.[search_by] LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctATC
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;
 
	-- Total count
	SET @QueryCount = 'SELECT COUNT( DISTINCT f.atc_PK)
						from (
						   select atccode, min(atc_PK) as atc_PKx
						   from [dbo].[ATC] group by atccode
						) as x inner join [dbo].[ATC] as f on f.atccode = x.atccode and f.atc_PK = x.atc_PKx
					' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
