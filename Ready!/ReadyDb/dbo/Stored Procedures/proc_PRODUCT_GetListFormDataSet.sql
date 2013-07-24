CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetListFormDataSet]
	@ProductName nvarchar(250) = NULL,
	@product_number nvarchar(250) = NULL,
	@AuthorisationProcedure nvarchar(250) = NULL,
	@Client nvarchar(250) = NULL,
	@Countries nvarchar(250) = NULL,
	@ActiveSubstances nvarchar(250) = NULL,

	@AuthProdCount nvarchar(250) = NULL,
	@PharProdCount nvarchar(250) = NULL,
	@ActCount nvarchar(250) = NULL,
	@SubUnitCount nvarchar(250) = NULL,
	@DocCount nvarchar(250) = NULL,
	@ResponsibleUserFk nvarchar(250) = NULL,

	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ProductName'
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
		Product.* FROM
		(
			SELECT DISTINCT
			prod.product_PK,
			prod.ProductName,
			prod.product_number,
			authProc.name as AuthorisationProcedure,
			client.name_org as Client,
			prod.Countries,
			prod.ActiveSubstances,
			prod.responsible_user_person_FK as ResponsibleUserFk,
			(select count(DISTINCT ap.ap_PK) 
				from dbo.AUTHORISED_PRODUCT ap
				where ap.product_FK = prod.product_PK) as AuthProdCount,
			(select count(DISTINCT pp.pp_FK) 
				from dbo.PRODUCT_PP_MN pp
				where pp.product_FK = prod.product_PK) as PharProdCount,
			(select count(DISTINCT ap.activity_FK) 
				from [dbo].[ACTIVITY_PRODUCT_MN] ap
				where ap.product_FK = prod.product_PK) as ActCount,
			(select COUNT(DISTINCT su.submission_unit_FK) 
				from dbo.PRODUCT_SUB_UNIT_MN as SU
				where product_FK = prod.product_PK) as SubUnitCount,
				(select count(DISTINCT pd.document_FK) 
				from dbo.PRODUCT_DOCUMENT_MN pd
				where pd.product_FK = prod.product_PK) as DocCount
			'
	IF @QueryBy = 'TimeUnit' 
		BEGIN 
			DECLARE @TimeUnitPk int = 0;
			DECLARE @ActivityPk int = 0;

			IF (ISNUMERIC(@EntityPk) = 1) SET @TimeUnitPk = CAST(@EntityPk AS INT);
			SET @ActivityPk = ISNULL((select top 1 tu.activity_FK from dbo.TIME_UNIT tu where tu.time_unit_PK = @TimeUnitPk), 0);

			SET @Query = @Query +
			'FROM dbo.ACTIVITY_PRODUCT_MN paMN
			 JOIN [dbo].[PRODUCT] prod ON prod.product_PK = paMN.product_FK
			 LEFT JOIN [dbo].[TYPE] authProc on authProc.type_PK = prod.authorisation_procedure
			 LEFT JOIN dbo.ORGANIZATION client on client.organization_PK = prod.client_organization_FK
			 WHERE paMN.activity_FK = ' +  CAST(@ActivityPk AS NVARCHAR(10)) + '
			'
		END
	ELSE SET @Query = @Query + 
			'FROM [dbo].[PRODUCT] prod
			 LEFT JOIN [dbo].[TYPE] authProc on authProc.type_PK = prod.authorisation_procedure
			 LEFT JOIN dbo.ORGANIZATION client on client.organization_PK = prod.client_organization_FK
			'
		SET @Query = @Query + 
		') AS Product 
		'
		SET @TempWhereQuery = '';

		-- @ProductName
		IF @ProductName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.ProductName LIKE N''%' + REPLACE(REPLACE(@ProductName,'[','[[]'),'''','''''') + '%'''
		END

		-- @product_number
		IF @product_number IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.product_number LIKE N''%' + REPLACE(REPLACE(@product_number,'[','[[]'),'''','''''') + '%'''
		END

		-- @AuthorisationProcedure
		IF @AuthorisationProcedure IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.AuthorisationProcedure LIKE N''' + REPLACE(REPLACE(@AuthorisationProcedure,'[','[[]'),'''','''''') + ''''
		END

		-- @Client
		IF @Client IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.Client LIKE N''%' + REPLACE(REPLACE(@Client,'[','[[]'),'''','''''') + '%'''
		END

		-- @Countries
		IF @Countries IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.Countries LIKE N''%' + REPLACE(REPLACE(@Countries,'[','[[]'),'''','''''') + '%'''
		END

		-- @ActiveSubstances
		IF @ActiveSubstances IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.ActiveSubstances LIKE N''%' + REPLACE(REPLACE(@ActiveSubstances,'[','[[]'),'''','''''') + '%'''
		END

		-- @AuthProdCount
		IF @AuthProdCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.AuthProdCount LIKE N''%' + REPLACE(REPLACE(@AuthProdCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @PharProdCount
		IF @PharProdCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.PharProdCount LIKE N''%' + REPLACE(REPLACE(@PharProdCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @ActCount
		IF @ActCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.ActCount LIKE N''%' + REPLACE(REPLACE(@ActCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @SubUnitCount
		IF @SubUnitCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.SubUnitCount LIKE N''%' + REPLACE(REPLACE(@SubUnitCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @DocCount
		IF @DocCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.DocCount LIKE N''%' + REPLACE(REPLACE(@DocCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Product.ResponsibleUserFk = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''') + ''
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