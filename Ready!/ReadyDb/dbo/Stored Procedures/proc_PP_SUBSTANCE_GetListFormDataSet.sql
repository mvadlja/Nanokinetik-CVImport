CREATE PROCEDURE  [dbo].[proc_PP_SUBSTANCE_GetListFormDataSet]
	@SubstanceName nvarchar(250) = NULL,
	@ConcentrationType nvarchar(250) = NULL,
	@LowNumValue nvarchar(250) = NULL,
	@LowNumPrefix nvarchar(250) = NULL,
	@LowNumUnit nvarchar(250) = NULL,
	@LowDenValue nvarchar(250) = NULL,
	@LowDenPrefix nvarchar(250) = NULL,
	@LowDenUnit nvarchar(250) = NULL,
	@HighNumValue nvarchar(250) = NULL,
	@HighNumPrefix nvarchar(250) = NULL,
	@HighNumUnit nvarchar(250) = NULL,
	@HighDenValue nvarchar(250) = NULL,
	@HighDenPrefix nvarchar(250) = NULL,
	@HighDenUnit nvarchar(250) = NULL,

	@SubstanceType nvarchar(50) = NULL,
	@SessionId nvarchar(50) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'SubstanceName'
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
	PPSubstance.* FROM
	(
		SELECT DISTINCT
		pps.ppsubstance_PK,
		pps.ppsubstance_FK,
		 CASE 
			WHEN pps.concise IS NOT NULL AND LTRIM(pps.concise) != '''' THEN pps.concise
			WHEN s.substance_name IS NOT NULL AND LTRIM(s.substance_name) != '''' THEN s.substance_name
			ELSE ''N/A''
		END AS SubstanceName,
		pps.lowamountnumervalue AS LowNumValue,
		pps.lowamountdenomvalue AS LowDenValue,
		pps.highamountnumervalue AS HighNumValue,
		pps.highamountdenomvalue AS HighDenValue,
		(	select  cv.term_name_english
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv 
			where cv.ssi__cont_voc_PK = pps.[concentrationtypecode]	) as ConcentrationType,
		(	select cv.term_name_english
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv
			where cv.ssi__cont_voc_PK = pps.[ExpressedBy_FK] ) as ExpressedBy,
		(	select cv.Field8
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv
			where cv.ssi__cont_voc_PK = pps.[lowamountnumerprefix]) as LowNumPrefix,
		(	select  cv.Field8
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv
			where cv.ssi__cont_voc_PK = pps.[lowamountnumerunit]) as LowNumUnit,
		(	select  cv.Field8
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv
			where cv.ssi__cont_voc_PK = pps.[lowamountdenomprefix]) as LowDenPrefix,
		case
			when ''Units of Presentation'' = ( select cv.term_name_english from SSI.dbo.SSI_CONTROLED_VOCABULARY cv where cv.ssi__cont_voc_PK = pps.[expressedby_FK] )
			then  ( select cv.[Description] from SSI.dbo.SSI_CONTROLED_VOCABULARY cv where cv.ssi__cont_voc_PK = pps.[lowamountdenomunit] )
			else ( select cv.[Field8] from SSI.dbo.SSI_CONTROLED_VOCABULARY cv where cv.ssi__cont_voc_PK = pps.[lowamountdenomunit] )
		end as LowDenUnit,
		(	select  cv.Field8
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv
			where cv.ssi__cont_voc_PK = pps.[highamountnumerprefix]) as HighNumPrefix,
		(	select  cv.Field8
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv
			where cv.ssi__cont_voc_PK = pps.[highamountnumerunit]) as HighNumUnit,
		(	select  cv.Field8
			from SSI.dbo.SSI_CONTROLED_VOCABULARY cv
			where cv.ssi__cont_voc_PK = pps.[highamountdenomprefix]) as HighDenPrefix,
		case
			when ''Units of Presentation'' = ( select cv.term_name_english from SSI.dbo.SSI_CONTROLED_VOCABULARY cv where cv.ssi__cont_voc_PK = pps.[expressedby_FK] )
			then  ( select cv.[Description] from SSI.dbo.SSI_CONTROLED_VOCABULARY cv where cv.ssi__cont_voc_PK = pps.[highamountdenomunit] )
			else ( select cv.[Field8] from SSI.dbo.SSI_CONTROLED_VOCABULARY cv where cv.ssi__cont_voc_PK = pps.[highamountdenomunit] )
		end as HighDenUnit,
		'''' as [Delete],
		'''' as [Errors]
		FROM dbo.PP_SUBSTANCE pps
		LEFT JOIN dbo.SUBSTANCES s ON s.substance_PK = pps.[substancecode_FK]
		WHERE pps.substancetype = ''' + ISNULL(@SubstanceType,' ') + ''' AND pps.sessionid = ''' + ISNULL(@SessionId,' ') + '''
		) AS PPSubstance 
	'
		SET @TempWhereQuery = '';

		-- @SubstanceName
		IF @SubstanceName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.SubstanceName LIKE N''%' + REPLACE(REPLACE(@SubstanceName,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @ConcentrationType
		IF @ConcentrationType IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.ConcentrationType LIKE N''%' + REPLACE(REPLACE(@ConcentrationType,'[','[[]'),'''','''''') + '%'''
		END

		-- @LowNumValue
		IF @LowNumValue IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.LowNumValue LIKE N''%' + REPLACE(REPLACE(@LowNumValue,'[','[[]'),'''','''''') + '%'''
		END

		-- @LowNumPrefix
		IF @LowNumPrefix IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.LowNumPrefix LIKE N''%' + REPLACE(REPLACE(@LowNumPrefix,'[','[[]'),'''','''''') + '%'''
		END

		-- @LowNumUnit
		IF @LowNumUnit IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.LowNumUnit LIKE N''%' + REPLACE(REPLACE(@LowNumUnit,'[','[[]'),'''','''''') + '%'''
		END

		-- @LowDenValue
		IF @LowDenValue IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.LowDenValue LIKE N''%' + REPLACE(REPLACE(@LowDenValue,'[','[[]'),'''','''''') + '%'''
		END

		-- @LowDenPrefix
		IF @LowDenPrefix IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.LowDenPrefix LIKE N''%' + REPLACE(REPLACE(@LowDenPrefix,'[','[[]'),'''','''''') + '%'''
		END

		-- @LowDenUnit
		IF @LowDenUnit IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.LowDenUnit LIKE N''%' + REPLACE(REPLACE(@LowDenUnit,'[','[[]'),'''','''''') + '%'''
		END

		-- @HighNumValue
		IF @HighNumValue IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.HighNumValue LIKE N''%' + REPLACE(REPLACE(@HighNumValue,'[','[[]'),'''','''''') + '%'''
		END

		-- @HighNumPrefix
		IF @HighNumPrefix IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.HighNumPrefix LIKE N''%' + REPLACE(REPLACE(@HighNumPrefix,'[','[[]'),'''','''''') + '%'''
		END

		-- @HighNumUnit
		IF @HighNumUnit IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.HighNumUnit LIKE N''%' + REPLACE(REPLACE(@HighNumUnit,'[','[[]'),'''','''''') + '%'''
		END

		-- @HighDenValue
		IF @HighDenValue IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.HighDenValue LIKE N''%' + REPLACE(REPLACE(@HighDenValue,'[','[[]'),'''','''''') + '%'''
		END

		-- @HighDenPrefix
		IF @HighDenPrefix IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.HighDenPrefix LIKE N''%' + REPLACE(REPLACE(@HighDenPrefix,'[','[[]'),'''','''''') + '%'''
		END

		-- @HighDenUnit
		IF @HighDenUnit IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PPSubstance.HighDenUnit LIKE N''%' + REPLACE(REPLACE(@HighDenUnit,'[','[[]'),'''','''''') + '%'''
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