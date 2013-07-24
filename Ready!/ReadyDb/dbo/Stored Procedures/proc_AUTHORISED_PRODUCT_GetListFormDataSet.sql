-- GetAuthorisedProductsDataSet
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_GetListFormDataSet]
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
	@ResponsibleUserFk nvarchar(250) = NULL,

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
	SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
	AuthorisedProduct.* FROM
	(
		SELECT DISTINCT
		ap.ap_PK,
		ISNULL(ap.product_name, ''Missing'') AS AuthorisedProductName,
		ap.ev_code AS evcode, 
		ap.product_FK,
		ap.authorisationnumber AS AuthorisationNumber,
		ap.authorisationdate AS AuthorisationDate,
		ap.authorisationexpdate AS AuthorisationExpiryDate,
		ap.packagedesc AS PackageDescription,
		ap.responsible_user_person_FK as ResponsibleUserFk,
		CASE
			WHEN ap.[article_57_reporting]=1 THEN ''Yes''
			WHEN ap.[article_57_reporting]=0 THEN ''No''
			ELSE ''''
		END AS Article57Relevant,
		p.ProductName AS RelatedProduct,
		p.ActiveSubstances,
		(	SELECT TOP 1 xs.name + '' (''+ xo.name + '')''
			FROM dbo.XEVPRM_ENTITY_DETAILS_MN xMN
			JOIN dbo.XEVPRM_AP_DETAILS xAp ON xAp.xevprm_ap_details_PK = xMN.xevprm_entity_details_FK
			JOIN dbo.XEVPRM_MESSAGE x ON xMN.xevprm_message_FK = x.xevprm_message_PK
			JOIN dbo.XEVPRM_MESSAGE_STATUS xs ON xs.xevprm_message_status_PK = x.message_status_FK
			LEFT JOIN dbo.XEVPRM_OPERATION_TYPE xo ON xo.xevprm_operation_type_PK = xAp.operation_type
			WHERE xMN.xevprm_entity_type_FK = 1 AND xAp.ap_FK = ap.ap_PK AND x.[Deleted] != 1
			ORDER BY xMN.xevprm_message_FK DESC ) as XevprmStatus,
		( SELECT COUNT(ad.document_FK) FROM [dbo].[AP_DOCUMENT_MN] ad WHERE ad.ap_FK = ap.ap_PK ) AS DocumentsCount,
		( SELECT c.abbreviation FROM [dbo].[COUNTRY] c WHERE c.country_PK = ap.authorisationcountrycode_FK ) AS Country,
		( SELECT t.name FROM [dbo].[TYPE] t WHERE t.type_PK = ap.authorisationstatus_FK ) AS AuthorisationStatus,
		( SELECT o.name_org FROM [dbo].[ORGANIZATION] o WHERE o.organization_PK = ap.organizationmahcode_FK ) AS LicenceHolder,
		( SELECT p.FullName FROM [dbo].[QPPV_CODE] q JOIN [dbo].[PERSON] p on q.person_FK = p.person_PK where q.qppv_code_PK = ap.local_qppv_code_FK) AS LocalQppv,
		( SELECT o.name_org FROM [dbo].[ORGANIZATION] o WHERE o.organization_PK = ap.mflcode_FK ) AS MasterFileLocation,
		( SELECT o.name_org FROM [dbo].[ORGANIZATION] o WHERE o.organization_PK = p.client_organization_FK ) AS Client
		'
		IF @QueryBy = 'Product' 
		BEGIN 
			SET @Query = @Query +
							'FROM [dbo].[AUTHORISED_PRODUCT] ap
							 LEFT JOIN dbo.PRODUCT p ON p.product_PK = ap.product_FK
							 WHERE p.product_PK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
							'
		END
		ELSE SET @Query = @Query + 
							'FROM [dbo].[AUTHORISED_PRODUCT] ap
							 LEFT JOIN dbo.PRODUCT p ON p.product_PK = ap.product_FK
							'
		SET @Query = @Query + 
		') AS AuthorisedProduct 
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

		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'AuthorisedProduct.ResponsibleUserFk = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''') + ''
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