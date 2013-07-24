

CREATE PROCEDURE [dbo].[proc_PERSON_GetListFormDataSet]
	@FullName nvarchar(250) = NULL,
	@EvCode nvarchar(250) = NULL,
	@Title nvarchar(250) = NULL,
	@Street nvarchar(250) = NULL,
	@Company nvarchar(250) = NULL,
	@State nvarchar(250) = NULL,
	@Department nvarchar(250) = NULL,
	@Postcode nvarchar(250) = NULL,
	@Building nvarchar(250) = NULL,
	@CoutryCode nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 200,
	@OrderByQuery nvarchar(1000) = 'person_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		PERSON.* FROM
		(
			SELECT DISTINCT
			person.person_PK,
			person.FullName,
			[dbo].[ReturnQppvCodesByPerson](person.person_PK) AS EvCode,
			person.title AS Title,
			person.street AS Street,
			person.company AS Company,
			person.state AS State,
			person.department AS Department,
			person.postcode AS Postcode,
			person.building AS Building,
			(country.abbreviation + ''-'' + country.name) AS CoutryCode,
			'''' as [Delete]

			FROM dbo.PERSON person
			LEFT JOIN Country AS country ON country.country_PK = person.country_FK
			'
		SET @Query = @Query + 
		') AS person
		'
		SET @TempWhereQuery = '';

		-- @FullName
		IF @FullName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.FullName LIKE N''%' + REPLACE(REPLACE(@FullName,'[','[[]'),'''','''''') + '%'''
		END

		-- @EvCode
		IF @EvCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.EvCode LIKE N''%' + REPLACE(REPLACE(@EvCode,'[','[[]'),'''','''''') + '%'''
		END

		-- @Title
		IF @Title IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.Title LIKE N''%' + REPLACE(REPLACE(@Title,'[','[[]'),'''','''''') + '%'''
		END

		-- @Street
		IF @Street IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.Street LIKE N''%' + REPLACE(REPLACE(@Street,'[','[[]'),'''','''''') + '%'''
		END

		-- @Company
		IF @Company IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.Company LIKE N''%' + REPLACE(REPLACE(@Company,'[','[[]'),'''','''''') + '%'''
		END

		-- @State
		IF @State IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.State LIKE N''%' + REPLACE(REPLACE(@State,'[','[[]'),'''','''''') + '%'''
		END

		-- @Department
		IF @Department IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.Department LIKE N''%' + REPLACE(REPLACE(@Department,'[','[[]'),'''','''''') + '%'''
		END

		-- @State
		IF @State IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.State LIKE N''%' + REPLACE(REPLACE(@State,'[','[[]'),'''','''''') + '%'''
		END

		-- @Postcode
		IF @Postcode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.Postcode LIKE N''%' + REPLACE(REPLACE(@Postcode,'[','[[]'),'''','''''') + '%'''
		END

		-- @Building
		IF @Building IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.Building LIKE N''%' + REPLACE(REPLACE(@Building,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @CoutryCode
		IF @CoutryCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'person.CoutryCode LIKE N''%' + REPLACE(REPLACE(@CoutryCode,'[','[[]'),'''','''''') + '%'''
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + 
	')
	
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END