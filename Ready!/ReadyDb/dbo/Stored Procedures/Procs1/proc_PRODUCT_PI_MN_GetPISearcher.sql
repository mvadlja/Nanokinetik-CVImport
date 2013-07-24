-- ProductPISearcher
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_GetPISearcher]
	@name nvarchar(100) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = ''
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
		DistinctPI.* FROM
		(
			SELECT DISTINCT
			pin.product_indications_PK AS ID, pin.name AS Name
			FROM [dbo].[PRODUCT_INDICATION] pin
			'
			SET @TempWhereQuery = '';

			-- Check nullability for every parameter
			-- @role_name
			--IF @role_name IS NOT NULL
			--BEGIN
			--	IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			--	ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			--	SET @TempWhereQuery = @TempWhereQuery + '[dbo].[ORG_TYPE_FOR_PARTNER].[org_type_name] LIKE ''' + CONVERT (nvarchar(MAX), @role_name) + ''''
			--END


			-- Check nullability for every parameter
			-- @name_org
			IF @name IS NOT NULL
			BEGIN
				IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
				ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
				SET @TempWhereQuery = @TempWhereQuery + 'pin.[name] LIKE ''%' + REPLACE(@name,'[','[[]') + '%'''
			END


			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctPI
	)
	

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;
 
	-- Total count
	SET @QueryCount = 'SELECT COUNT(DISTINCT pin.product_indications_PK)FROM [dbo].[PRODUCT_INDICATION] pin
					' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
	
END
