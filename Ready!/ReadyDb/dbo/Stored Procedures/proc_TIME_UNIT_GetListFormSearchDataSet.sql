CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_GetListFormSearchDataSet]
	@SearchTimeUnitNamePk nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchActivityPk nvarchar(250) = NULL,
	@SearchActualDateFrom nvarchar(250) = NULL,
	@SearchActualDateTo nvarchar(250) = NULL,

	@TimeUnitName nvarchar(250) = NULL,
	@ResponsibleUser nvarchar(250) = NULL,
	@actual_date nvarchar(250) = NULL,
	@Time nvarchar(250) = NULL,
	@description nvarchar(250) = NULL,
	@Activity nvarchar(250) = NULL,
	@ActivityDescription nvarchar(250) = NULL,
	@Products nvarchar(250) = NULL,
	@InsertedBy nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'TimeUnitName'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT DISTINCT time_unit_PK, TimeUnitName, ResponsibleUser, actual_date, [Time], [description], Activity, ActivityDescription, Products, InsertedBy, activity_FK
		FROM
		(
			SELECT TimeUnit.* 
			FROM
			(
				SELECT DISTINCT
				tu.time_unit_PK,
				tu.actual_date,
				tu.[description],
				tu.user_FK,
				tu.activity_FK,
			    tu.time_unit_name_FK,
				tun.time_unit_name AS TimeUnitName,
				a.name AS Activity,
				a.[description] AS ActivityDescription,

				(ISNULL(rp.name,'''') + '' '' + ISNULL(rp.familyname,'''')) AS ResponsibleUser,
				(ISNULL(ip.name,'''') + '' '' + ISNULL(ip.familyname,'''')) AS InsertedBy,

				CASE
					WHEN tu.[time_hours] is null THEN ''00''
					WHEN tu.[time_hours] < 10 THEN ''0'' + CAST(tu.[time_hours] AS NVARCHAR(10))
					ELSE CAST(tu.[time_hours] AS NVARCHAR(10))
				END + '':'' +
				CASE
					WHEN tu.[time_minutes] is null THEN ''00''
					WHEN tu.[time_minutes] < 10 THEN ''0'' + CAST(tu.[time_minutes] AS NVARCHAR(10))
					ELSE CAST(tu.[time_minutes] AS NVARCHAR(10))
				END AS [Time],
			
				STUFF ( (
					SELECT CAST('' ||| '' AS NVARCHAR(MAX)) + ProductTable.ProductName
					FROM (
						SELECT DISTINCT
						p.ProductName + '' || '' + CAST(p.product_PK AS NVARCHAR(50)) AS ProductName
						from [dbo].[ACTIVITY_PRODUCT_MN]  paMn 
						JOIN [dbo].[PRODUCT] p ON p.[product_PK] = paMn.[product_FK]
						WHERE tu.activity_FK IS NOT NULL AND paMn.activity_FK = tu.activity_FK
					) AS ProductTable 
					for xml path('''') ), 1, 5, '''') AS Products

				FROM [dbo].[TIME_UNIT] tu
				LEFT JOIN [dbo].[PERSON] rp ON rp.person_PK = tu.[user_FK]
				LEFT JOIN [dbo].[PERSON] ip ON ip.person_PK = tu.[inserted_by]
				LEFT JOIN [dbo].[TIME_UNIT_NAME] tun ON tun.time_unit_name_PK = tu.[time_unit_name_FK]
				LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = tu.[activity_FK]'

		SET @Query = @Query + '
			) AS TimeUnit 
			'
		SET @TempWhereQuery = '';

		-- @TimeUnitName
		IF @TimeUnitName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.TimeUnitName LIKE N''%' + REPLACE(REPLACE(@TimeUnitName,'[','[[]'),'''','''''') + '%'''
		END

		-- @ResponsibleUser
		IF @ResponsibleUser IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.ResponsibleUser LIKE N''%' + REPLACE(REPLACE(@ResponsibleUser,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @actual_date
		IF @actual_date IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30),TimeUnit.[actual_date],104) LIKE ''%' + REPLACE(REPLACE(@actual_date,'[','[[]'),'''','''''') + '%'''
		END	

		-- @Time
		IF @Time IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.[Time] LIKE N''%' + REPLACE(REPLACE(@Time,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @description
		IF @description IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.[description] LIKE N''%' + REPLACE(REPLACE(@description,'[','[[]'),'''','''''') + '%'''
		END

		-- @Activity
		IF @Activity IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.Activity LIKE N''%' + REPLACE(REPLACE(@Activity,'[','[[]'),'''','''''') + '%'''
		END

		-- @ActivityDescription
		IF @ActivityDescription IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.ActivityDescription LIKE N''%' + REPLACE(REPLACE(@ActivityDescription,'[','[[]'),'''','''''') + '%'''
		END

		-- @Products
		IF @Products IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.Products LIKE N''%' + REPLACE(REPLACE(@Products,'[','[[]'),'''','''''') + '%'''
		END

		-- @InsertedBy
		IF @InsertedBy IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.InsertedBy LIKE N''%' + REPLACE(REPLACE(@InsertedBy,'[','[[]'),'''','''''') + '%'''
		END

		------------------------------------------------------------------------SEARCH------------------------------------------------------------------------

		-- @SearchTimeUnitNamePk
		IF @SearchTimeUnitNamePk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.time_unit_name_FK LIKE N''%' + REPLACE(REPLACE(@SearchTimeUnitNamePk,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchResponsibleUserPk
		IF @SearchResponsibleUserPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.user_FK = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''')
		END

		-- @SearchActivityPk
		IF @SearchActivityPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.activity_FK = ' + REPLACE(REPLACE(@SearchActivityPk,'[','[[]'),'''','''''')
		END

		-- @SearchActualDateFrom
		IF @SearchActualDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.[actual_date] >= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchActualDateTo
		IF @SearchActualDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'TimeUnit.[actual_date] <= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + '
		) DistinctResult
	)
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,*
		FROM PagingResult
	)x
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END