-- GetAuthorisedProductsDataSet
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_GetListFormSearchDataSet]
	@SearchRelatedProductPk nvarchar(250) = NULL,
	@SearchArcticle57 nvarchar(250) = NULL,
	@SearchSubstanceTranslations nvarchar(250) = NULL,
	@SearchEvcode nvarchar(250) = NULL,
	@SearchFullPresentationName nvarchar(250) = NULL,
	@SearchPackageDescription nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchMarketed nvarchar(250) = NULL,
	@SearchLegalStatusPk nvarchar(250) = NULL,
	@SearchLicenceHolderPk nvarchar(250) = NULL,
	@SearchLocalRepresentativePk nvarchar(250) = NULL,
	@SearchQppvPk nvarchar(250) = NULL,
	@SearchLocalQppvPk nvarchar(250) = NULL,
	@SearchMasterFileLocationPk nvarchar(250) = NULL,
	@SearchIndications nvarchar(250) = NULL,
	@SearchAuthorisationCountryPk nvarchar(250) = NULL,
	@SearchAuthorisationStatusPk nvarchar(250) = NULL,
	@SearchClientPk nvarchar(250) = NULL,
	@SearchAuthorisationDateFrom nvarchar(250) = NULL,
	@SearchAuthorisationDateTo nvarchar(250) = NULL,
	@SearchAuthorisationExpiryDateFrom nvarchar(250) = NULL,
	@SearchAuthorisationExpiryDateTo nvarchar(250) = NULL,
	@SearchSunsetClauseFrom nvarchar(250) = NULL,
	@SearchSunsetClauseTo nvarchar(250) = NULL,
	
	@AuthorisedProductName nvarchar(250) = null,
	@Client nvarchar(250) = null,
	@evcode nvarchar(250) = null,
	@XevprmStatus nvarchar(250) = null,
	@Article57Relevant nvarchar(250) = null,
	@PackageDescription nvarchar(250) = null,
	@Country nvarchar(250) = null,
	@RelatedProduct nvarchar(250) = null,
	@AuthorisationStatus nvarchar(250) = null,
	@AuthorisationNumber nvarchar(250) = null,
	@AuthorisationDate nvarchar(250) = null,
	@LicenceHolder nvarchar(250) = null,
	@MasterFileLocation nvarchar(250) = null,
	@LocalQppv nvarchar(250) = null,
	@AuthorisationExpiryDate nvarchar(250) = null,
	@DocumentsCount nvarchar(250) = null,
	
	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ap_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
WITH PagingResult AS
(
	SELECT DISTINCT ap_PK, product_FK, AuthorisedProductName, Client, evcode, PackageDescription, Country, AuthorisationNumber, AuthorisationDate, LicenceHolder, AuthorisationExpiryDate, DocumentsCount, 
		Article57Relevant, AuthorisationStatus, XevprmStatus, RelatedProduct, ActiveSubstances, LocalQppv, MasterFileLocation
		FROM
		(
			SELECT AuthorisedProduct.* 
			FROM
			(
				SELECT DISTINCT
				ap.ap_PK,
				ISNULL(ap.product_name, ''Missing'') AS AuthorisedProductName,
				ap.ev_code AS evcode, 
				ap.product_FK,
				ap.responsible_user_person_FK,
				ap.legalstatus,
				ap.authorisationnumber AS AuthorisationNumber,
				ap.authorisationdate AS AuthorisationDate,
				ap.authorisationexpdate AS AuthorisationExpiryDate,
				ap.packagedesc AS PackageDescription,
				CASE
					WHEN ap.[article_57_reporting]=1 THEN ''Yes''
					WHEN ap.[article_57_reporting]=0 THEN ''No''
					ELSE ''''
				END AS Article57Relevant,
				CASE
					WHEN ap.[marketed]=1 THEN ''Yes''
					WHEN ap.[marketed]=0 THEN ''No''
					ELSE ''''
				END AS Marketed,
				CASE
					WHEN ap.[substance_translations]=1 THEN ''Yes''
					WHEN ap.[substance_translations]=0 THEN ''No''
					ELSE ''''
				END AS SubstanceTranslations,
				ap.[article_57_reporting] AS IsArticle57,
				p.ProductName AS RelatedProduct,
				p.client_organization_FK AS ClientPk,
				ap.authorisationcountrycode_FK,
				ap.authorisationstatus_FK,
				ap.local_representative_FK,
				ap.mflcode_FK,
				ap.sunsetclause as SunsetClause,
				p.ActiveSubstances,
				(	SELECT TOP 1 xs.name + '' (''+ xo.name + '')''
					FROM dbo.XEVPRM_ENTITY_DETAILS_MN xMN
					JOIN dbo.XEVPRM_AP_DETAILS xAp ON xAp.xevprm_ap_details_PK = xMN.xevprm_entity_details_FK
					JOIN dbo.XEVPRM_MESSAGE x ON xMN.xevprm_message_FK = x.xevprm_message_PK
					JOIN dbo.XEVPRM_MESSAGE_STATUS xs ON xs.xevprm_message_status_PK = x.message_status_FK
					LEFT JOIN dbo.XEVPRM_OPERATION_TYPE xo ON xo.xevprm_operation_type_PK = xAp.operation_type
					WHERE xMN.xevprm_entity_type_FK = 1 AND xAp.ap_FK = ap.ap_PK AND x.[Deleted] != 1
					ORDER BY xMN.xevprm_message_FK DESC ) as XevprmStatus,
				ap.Indications,
				( SELECT COUNT(ad.document_FK) FROM [dbo].[AP_DOCUMENT_MN] ad WHERE ad.ap_FK = ap.ap_PK ) AS DocumentsCount,
				( SELECT c.abbreviation FROM [dbo].[COUNTRY] c WHERE c.country_PK = ap.authorisationcountrycode_FK ) AS Country,
				( SELECT t.name FROM [dbo].[TYPE] t WHERE t.type_PK = ap.authorisationstatus_FK ) AS AuthorisationStatus,
				( SELECT o.name_org FROM [dbo].[ORGANIZATION] o WHERE o.organization_PK = ap.organizationmahcode_FK ) AS LicenceHolder,
				( SELECT o.organization_PK FROM [dbo].[ORGANIZATION] o WHERE o.organization_PK = ap.organizationmahcode_FK ) AS LicenceHolderPk,
				( SELECT o.name_org FROM [dbo].[ORGANIZATION] o WHERE o.organization_PK = p.client_organization_FK ) AS Client,
				( SELECT qppvCode.person_FK FROM [dbo].[QPPV_CODE] qppvCode WHERE qppvCode.qppv_code_PK = ap.qppv_code_FK ) AS QppvPk,
				( SELECT qppvCode.person_FK FROM [dbo].[QPPV_CODE] qppvCode WHERE qppvCode.qppv_code_PK = ap.local_qppv_code_FK ) AS LocalQppvPk,
				( SELECT p.FullName FROM [dbo].[QPPV_CODE] q JOIN [dbo].[PERSON] p on q.person_FK = p.person_PK where q.qppv_code_PK = ap.local_qppv_code_FK) AS LocalQppv,
				( SELECT o.name_org FROM [dbo].[ORGANIZATION] o WHERE o.organization_PK = ap.mflcode_FK ) AS MasterFileLocation'

		IF @QueryBy = 'Product' 
		BEGIN 
			SET @Query = @Query + '
				FROM [dbo].[AUTHORISED_PRODUCT] ap
				LEFT JOIN dbo.PRODUCT p ON p.product_PK = ap.product_FK
				WHERE p.product_PK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END
		END
		ELSE SET @Query = @Query + '
				FROM [dbo].[AUTHORISED_PRODUCT] ap
				LEFT JOIN dbo.PRODUCT p ON p.product_PK = ap.product_FK'

		SET @Query = @Query + '
			) AS AuthorisedProduct 
			'
		SET @TempWhereQuery = '';

		-- Check nullability for every parameter
		-- @AuthorisedProductName
		IF @AuthorisedProductName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.AuthorisedProductName LIKE ''%' +REPLACE(REPLACE(@AuthorisedProductName,'[','[[]'),'''','''''') + '%'''
		END
			
		-- @Client
		IF @Client IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.Client LIKE ''%' + REPLACE(REPLACE(@Client,'[','[[]'),'''','''''') + '%'''
		END	
			
		-- @evcode
		IF @evcode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.[evcode] LIKE ''%' + REPLACE(REPLACE(@evcode,'[','[[]'),'''','''''')  + '%'''
		END		
						
		-- @PackageDescription
		IF @PackageDescription IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.[PackageDescription] LIKE ''%' + REPLACE(REPLACE(@PackageDescription,'[','[[]'),'''','''''')  + '%'''
		END		
						
		-- @Country
		IF @Country IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.Country LIKE ''%' + REPLACE(REPLACE(@Country,'[','[[]'),'''','''''')  + '%'''
		END
			
		-- @AuthorisationNumber 
		IF @AuthorisationNumber  IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.[AuthorisationNumber] LIKE ''%' + REPLACE(REPLACE(@AuthorisationNumber ,'[','[[]'),'''','''''')  + '%'''
		END		

		-- @AuthorisationDate
		IF @AuthorisationDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30),AuthorisedProduct.[AuthorisationDate],104) LIKE ''%' + REPLACE(REPLACE(@AuthorisationDate,'[','[[]'),'''','''''') + '%'''
		END	
			
		-- @LicenceHolder
		IF @LicenceHolder IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.LicenceHolder LIKE ''%' + REPLACE(@LicenceHolder,'[','[[]') + '%'''
		END								

		-- @LocalQppv
		IF @LocalQppv IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.LocalQppv LIKE ''%' + REPLACE(@LocalQppv,'[','[[]') + '%'''
		END	

		-- @MasterFileLocation
		IF @MasterFileLocation IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.MasterFileLocation LIKE ''%' + REPLACE(@MasterFileLocation,'[','[[]') + '%'''
		END	

		-- @AuthorisationExpiryDate
		IF @AuthorisationExpiryDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30),AuthorisedProduct.AuthorisationExpiryDate,104) LIKE ''%' + REPLACE(REPLACE(@AuthorisationExpiryDate,'[','[[]'),'''','''''') + '%'''
		END	
			
		-- @DocumentsCount
		IF @DocumentsCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.DocumentsCount LIKE ''%' + CONVERT (nvarchar(MAX), REPLACE(REPLACE(@DocumentsCount,'[','[[]'),'''','''''') )+'%'''
		END		

		-- @Article57Relevant
		IF @Article57Relevant IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.Article57Relevant LIKE ''' + REPLACE(REPLACE(@Article57Relevant,'[','[[]'),'''','''''') + ''''
		END

		-- @AuthorisationStatus
		IF @AuthorisationStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.AuthorisationStatus = ''' + REPLACE(REPLACE(@AuthorisationStatus,'[','[[]'),'''','''''') + ''''
		END
		
		-- @XevprmStatus
		IF @XevprmStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.XevprmStatus LIKE N''%' + REPLACE(REPLACE(@XevprmStatus,'[','[[]'),'''','''''') + '%'''
		END

		-- @RelatedProduct
		IF @RelatedProduct IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.RelatedProduct LIKE ''%' + REPLACE(REPLACE(@RelatedProduct,'[','[[]'),'''','''''')  + '%'''
		END	
		
		------------------------------------------------------------------------SEARCH------------------------------------------------------------------------
		
		-- @SearchRelatedProductPk
		IF @SearchRelatedProductPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.product_FK = ' + REPLACE(REPLACE(@SearchRelatedProductPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchArcticle57
		IF @SearchArcticle57 IS NOT NULL AND @SearchArcticle57 != ''
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.Article57Relevant = ''' + REPLACE(REPLACE(@SearchArcticle57,'[','[[]'),'''','''''') + ''''
		END
		
		-- @SearchSubstanceTranslations
		IF @SearchSubstanceTranslations IS NOT NULL AND @SearchSubstanceTranslations != ''
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.SubstanceTranslations = ''' + REPLACE(REPLACE(@SearchSubstanceTranslations,'[','[[]'),'''','''''') + ''''
		END
		
		-- @SearchEvcode
		IF @SearchEvcode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.evcode LIKE N''%' + REPLACE(REPLACE(@SearchEvcode,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchFullPresentationName
		IF @SearchFullPresentationName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.AuthorisedProductName LIKE N''%' + REPLACE(REPLACE(@SearchFullPresentationName,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchPackageDescription
		IF @SearchPackageDescription IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.PackageDescription LIKE N''%' + REPLACE(REPLACE(@SearchPackageDescription,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchResponsibleUserPk
		IF @SearchResponsibleUserPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.responsible_user_person_FK = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchMarketed
		IF @SearchMarketed IS NOT NULL AND @SearchMarketed != ''
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.Marketed = ''' + REPLACE(REPLACE(@SearchMarketed,'[','[[]'),'''','''''') + ''''
		END
		
		-- @SearchLegalStatusPk
		IF @SearchLegalStatusPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.legalstatus = ' + REPLACE(REPLACE(@SearchLegalStatusPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchLicenceHolderPk
		IF @SearchLicenceHolderPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.LicenceHolderPk = ' + REPLACE(REPLACE(@SearchLicenceHolderPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchLocalRepresentativePk
		IF @SearchLocalRepresentativePk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.local_representative_FK = ' + REPLACE(REPLACE(@SearchLocalRepresentativePk,'[','[[]'),'''','''''')
		END
		
		-- @SearchMasterFileLocationPk
		IF @SearchMasterFileLocationPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.mflcode_FK = ' + REPLACE(REPLACE(@SearchMasterFileLocationPk,'[','[[]'),'''','''''')
		END
		-- @SearchQppvPk
		IF @SearchQppvPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.QppvPk = ' + REPLACE(REPLACE(@SearchQppvPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchLocalQppvPk
		IF @SearchLocalQppvPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.LocalQppvPk = ' + REPLACE(REPLACE(@SearchLocalQppvPk,'[','[[]'),'''','''''')
		END

	    -- @SearchIndications
		IF @SearchIndications IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.Indications LIKE N''%' + REPLACE(REPLACE(@SearchIndications,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchAuthorisationCountryPk
		IF @SearchAuthorisationCountryPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.authorisationcountrycode_FK = ' + REPLACE(REPLACE(@SearchAuthorisationCountryPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchAuthorisationStatusPk
		IF @SearchAuthorisationStatusPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.authorisationstatus_FK = ' + REPLACE(REPLACE(@SearchAuthorisationStatusPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchClientPk
		IF @SearchClientPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.ClientPk = ' + REPLACE(REPLACE(@SearchClientPk,'[','[[]'),'''','''''')
		END

		-- @SearchAuthorisationDateFrom
		IF @SearchAuthorisationDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.AuthorisationDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchAuthorisationDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
		
		-- @SearchAuthorisationDateTo
		IF @SearchAuthorisationDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.AuthorisationDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchAuthorisationDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END
		
		-- @SearchAuthorisationExpiryDateFrom
		IF @SearchAuthorisationExpiryDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.AuthorisationExpiryDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchAuthorisationExpiryDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
		
		-- @SearchAuthorisationExpiryDateTo
		IF @SearchAuthorisationExpiryDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.AuthorisationExpiryDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchAuthorisationExpiryDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END
		
		-- @@SearchSunsetClauseFrom
		IF @SearchSunsetClauseFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.SunsetClause >= convert(datetime, ''' + REPLACE(REPLACE(@SearchSunsetClauseFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
		
		-- @SearchSunsetClauseTo
		IF @SearchSunsetClauseTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.SunsetClause <= convert(datetime, ''' + REPLACE(REPLACE(@SearchSunsetClauseTo,'[','[[]'),'''','''''') + ''', 104) '
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