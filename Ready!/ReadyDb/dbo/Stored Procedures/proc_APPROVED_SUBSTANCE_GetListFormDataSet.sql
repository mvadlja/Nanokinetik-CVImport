




CREATE PROCEDURE [dbo].[proc_APPROVED_SUBSTANCE_GetListFormDataSet]
	@SubstanceName nvarchar(250) = NULL,
	@EvCode nvarchar(250) = NULL,
	@CasNumber nvarchar(250) = NULL,
	@MolecularFormula nvarchar(250) = NULL,
	@Class nvarchar(250) = NULL,
	@CDB nvarchar(250) = NULL,
	@Comment nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'approved_substance_PK'
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
		APPROVED_SUBSTANCE.* FROM
		(
			SELECT DISTINCT
			approvedSubstances.approved_substance_PK,
			approvedSubstances.substancename AS SubstanceName,
			approvedSubstances.ev_code AS EvCode,
			approvedSubstances.casnumber AS CasNumber,
			approvedSubstances.molecularformula AS MolecularFormula,
			approvedSubstances.class AS Class,
			approvedSubstances.cbd AS CDB,
			approvedSubstances.comments AS Comment,
			'''' as [Delete]

			FROM dbo.APPROVED_SUBSTANCE approvedSubstances
			'
		SET @Query = @Query + 
		') AS APPROVED_SUBSTANCE
		'
		SET @TempWhereQuery = '';

		-- @SubstanceName
		IF @SubstanceName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'APPROVED_SUBSTANCE.SubstanceName LIKE N''%' + REPLACE(REPLACE(@SubstanceName,'[','[[]'),'''','''''') + '%'''
		END

		-- @EvCode
		IF @EvCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'APPROVED_SUBSTANCE.EvCode LIKE N''%' + REPLACE(REPLACE(@EvCode,'[','[[]'),'''','''''') + '%'''
		END

		-- @CasNumber
		IF @CasNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'APPROVED_SUBSTANCE.CasNumber LIKE N''%' + REPLACE(REPLACE(@CasNumber,'[','[[]'),'''','''''') + '%'''
		END

		-- @MolecularFormula
		IF @MolecularFormula IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'APPROVED_SUBSTANCE.MolecularFormula LIKE N''%' + REPLACE(REPLACE(@MolecularFormula,'[','[[]'),'''','''''') + '%'''
		END

		-- @Class
		IF @Class IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'APPROVED_SUBSTANCE.Class LIKE N''%' + REPLACE(REPLACE(@Class,'[','[[]'),'''','''''') + '%'''
		END

		-- @CDB
		IF @CDB IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'APPROVED_SUBSTANCE.CDB LIKE N''%' + REPLACE(REPLACE(@CDB,'[','[[]'),'''','''''') + '%'''
		END

		-- @Comment
		IF @Comment IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'APPROVED_SUBSTANCE.Comment LIKE N''%' + REPLACE(REPLACE(@Comment,'[','[[]'),'''','''''') + '%'''
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