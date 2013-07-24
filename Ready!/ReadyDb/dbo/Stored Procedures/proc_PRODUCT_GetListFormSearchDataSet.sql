
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetListFormSearchDataSet]
	@SearchProductName nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchPharmaceuticalProductPk nvarchar(250) = NULL,
	@SearchProductNumber nvarchar(250) = NULL,
	@SearchAuthorisationProcedurePk nvarchar(250) = NULL,
	@SearchDrugAtcs nvarchar(250) = NULL,
	@SearchActiveSubstancePK nvarchar(250) = NULL,
	@SearchClient nvarchar(250) = NULL,
	@SearchDomainPk nvarchar(250) = NULL,
	@SearchTypePk nvarchar(250) = NULL,
	@SearchProductId nvarchar(250) = NULL,
	@SearchCountryPk nvarchar(250) = NULL,
	@SearchManufacturerPk nvarchar(250) = NULL,
	@SearchPsurCycle nvarchar(250) = NULL,
	@SearchArcticle57 nvarchar(250) = NULL,
	@SearchNextDlpFrom nvarchar(250) = NULL,
	@SearchNextDlpTo nvarchar(250) = NULL,
	
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

	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20000,
	@OrderByQuery nvarchar(1000) = 'ProductName'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWherePrimaryQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);


BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	
	WITH PagingResult AS
	(
		SELECT DISTINCT product_PK, ISNULL(ProductName, ''Missing'') AS ProductName, product_number, AuthorisationProcedure, Client, Article57Relevant, Countries, ActiveSubstances, AuthProdCount, PharProdCount, ActCount, SubUnitCount, DocCount
		FROM
		(
			SELECT Product.* 
			FROM
			(
				SELECT DISTINCT
				prod.product_PK,
				prod.ProductName,
				prod.product_number,
				prod.responsible_user_person_FK,
				prod.authorisation_procedure,
				prod.type_product_FK,
				prod.product_ID,
				prod.psur,
				prod.next_dlp,
				authProc.name as AuthorisationProcedure,
				client.name_org as Client,
				prod.Countries,
				prod.ActiveSubstances,
				prod.DrugAtcs,
				ppMN.pp_FK,
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
					where pd.product_FK = prod.product_PK) as DocCount,
				(SELECT 
					CASE 
						WHEN COUNT (CASE WHEN ap.[article_57_reporting]=1 THEN ''Yes'' END) > 0 THEN ''Yes''
						WHEN COUNT (CASE WHEN ap.[article_57_reporting]=0 THEN ''No'' END) > 0 THEN ''No''
					 END 
				FROM dbo.AUTHORISED_PRODUCT ap 
				WHERE ap.product_FK = prod.product_PK) AS Article57Relevant'
	IF @QueryBy = 'ByTimeUnit' 
	BEGIN 
		SET @Query = @Query + '
				FROM [dbo].[PRODUCT] prod
				JOIN dbo.ACTIVITY_PRODUCT_MN paMN on paMN.product_FK = prod.product_PK
				JOIN dbo.TIME_UNIT tu ON tu.activity_FK = paMn.activity_FK
				LEFT JOIN dbo.PRODUCT_PP_MN ppMN on ppMN.product_FK = prod.product_PK
				LEFT JOIN [dbo].[TYPE] authProc on authProc.type_PK = prod.authorisation_procedure
				LEFT JOIN dbo.ORGANIZATION client on client.organization_PK = prod.client_organization_FK
				' 
	END
	ELSE SET @Query = @Query + '
				FROM [dbo].[PRODUCT] prod
				LEFT JOIN dbo.PRODUCT_PP_MN ppMN on ppMN.product_FK = prod.product_PK
				LEFT JOIN [dbo].[TYPE] authProc on authProc.type_PK = prod.authorisation_procedure
				LEFT JOIN dbo.ORGANIZATION client on client.organization_PK = prod.client_organization_FK
				'

				SET @TempWherePrimaryQuery = '';

	-- @QueryBy
	IF @QueryBy = 'ByTimeUnit' 
	BEGIN
		IF LEN(@TempWherePrimaryQuery) = 0 BEGIN SET @TempWherePrimaryQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWherePrimaryQuery = @TempWherePrimaryQuery + ' AND '; END
		SET @TempWherePrimaryQuery = @TempWherePrimaryQuery + 'tu.time_unit_PK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END
	END 

	------------------------------------------------------------------------SEARCH------------------------------------------------------------------------

	-- @SearchActiveSubstancePK
	IF @SearchActiveSubstancePK IS NOT NULL
	BEGIN
		IF LEN(@TempWherePrimaryQuery) = 0 BEGIN SET @TempWherePrimaryQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWherePrimaryQuery = @TempWherePrimaryQuery + ' AND '; END
		SET @TempWherePrimaryQuery = @TempWherePrimaryQuery + 'prod.product_PK IN (SELECT p.product_PK from PRODUCT p
	left join PRODUCT_PP_MN ppMn ON ppmn.product_FK = p.product_PK
	left join PP_ACTIVE_INGREDIENT a ON a.pp_FK = ppmn.pp_FK
	where a.substancecode_FK IN ('+ @SearchActiveSubstancePK +')
	group by p.product_PK
	having count (DISTINCT a.substancecode_FK) = ' + CAST(len(@SearchActiveSubstancePK) - len(replace(@SearchActiveSubstancePK,',','')) + 1 AS NVARCHAR(3))+')'
	END 

	IF LEN(@TempWherePrimaryQuery) > 0 BEGIN SET @Query = @Query + @TempWherePrimaryQuery; END

	SET @Query = @Query + '
			) AS Product 
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

	------------------------------------------------------------------------SEARCH------------------------------------------------------------------------

	-- @SearchProductName
	IF @SearchProductName IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.ProductName LIKE N''%' + REPLACE(REPLACE(@SearchProductName,'[','[[]'),'''','''''') + '%'''
	END

	-- @SearchResponsibleUserPk
	IF @SearchResponsibleUserPk IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.responsible_user_person_FK = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''')
	END

	-- @SearchPharmaceuticalProductPk
	IF @SearchPharmaceuticalProductPk IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.pp_FK = ' + REPLACE(REPLACE(@SearchPharmaceuticalProductPk,'[','[[]'),'''','''''')
	END

	-- @SearchProductNumber
	IF @SearchProductNumber IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.product_number LIKE N''%' + REPLACE(REPLACE(@SearchProductNumber,'[','[[]'),'''','''''') + '%'''
	END

	-- @SearchAuthorisationProcedurePk
	IF @SearchAuthorisationProcedurePk IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.authorisation_procedure = ' + REPLACE(REPLACE(@SearchAuthorisationProcedurePk,'[','[[]'),'''','''''')
	END
	
	-- @SearchDrugAtcs
	IF @SearchDrugAtcs IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.DrugAtcs LIKE N''%' + REPLACE(REPLACE(@SearchDrugAtcs,'[','[[]'),'''','''''') + '%'''
	END

	-- @SearchClient
	IF @SearchClient IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.Client LIKE N''%' + REPLACE(REPLACE(@SearchClient,'[','[[]'),'''','''''') + '%'''
	END

	-- @SearchDomainPk
	IF @SearchDomainPk IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchDomainPk,'[','[[]'),'''','''''') + ' in (SELECT pdMN.domain_FK from dbo.PRODUCT_DOMAIN_MN pdMN WHERE pdMN.product_FK = Product.product_PK AND pdMN.domain_FK IS NOT NULL)'
	END

	-- @SearchTypePk
	IF @SearchTypePk IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.type_product_FK = ' + REPLACE(REPLACE(@SearchTypePk,'[','[[]'),'''','''''')
	END

	-- @SearchProductId
	IF @SearchProductId IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.product_ID LIKE N''%' + REPLACE(REPLACE(@SearchProductId,'[','[[]'),'''','''''') + '%'''
	END

	-- @SearchCountryPk
	IF @SearchCountryPk IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchCountryPk,'[','[[]'),'''','''''') + ' in (SELECT pcMN.country_FK from dbo.PRODUCT_COUNTRY_MN pcMN WHERE pcMN.product_FK = Product.product_PK AND pcMN.country_FK IS NOT NULL)'
	END

	-- @SearchManufacturerPk
	IF @SearchManufacturerPk IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchManufacturerPk,'[','[[]'),'''','''''') + ' in (SELECT m.organization_FK from dbo.ORG_IN_TYPE_FOR_MANUFACTURER m WHERE m.product_FK = Product.product_PK AND m.organization_FK IS NOT NULL)'
	END

	-- @SearchPsurCycle
	IF @SearchPsurCycle IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.psur LIKE N''%' + REPLACE(REPLACE(@SearchPsurCycle,'[','[[]'),'''','''''') + '%'''
	END

	-- @SearchArcticle57
	IF @SearchArcticle57 IS NOT NULL AND @SearchArcticle57 != ''
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.Article57Relevant = ''' + REPLACE(REPLACE(@SearchArcticle57,'[','[[]'),'''','''''') +''''
	END

	-- @SearchNextDlpFrom
	IF @SearchNextDlpFrom IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.next_dlp >= convert(datetime, ''' + REPLACE(REPLACE(@SearchNextDlpFrom,'[','[[]'),'''','''''') + ''', 104)'
	END
			
	-- @SearchNextDlpTo
	IF @SearchNextDlpTo IS NOT NULL
	BEGIN
		IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
		ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
		SET @TempWhereQuery = @TempWhereQuery + 'Product.next_dlp <= convert(datetime, ''' + REPLACE(REPLACE(@SearchNextDlpTo,'[','[[]'),'''','''''') + ''', 104)'
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

	print @ExecuteQuery
END