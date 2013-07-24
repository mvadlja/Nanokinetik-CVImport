CREATE PROCEDURE  [dbo].[proc_XEVPRM_MESSAGE_GetListFormDataSet]
	@message_number nvarchar(250) = NULL,
	@AuthorisedProduct nvarchar(250) = NULL,
	@PackageDescription nvarchar(250) = NULL,
	@AuthorisationNumber nvarchar(250) = NULL,
	@Country nvarchar(250) = NULL,
	@Product nvarchar(250) = NULL,
	@LicenseHolder nvarchar(250) = NULL,
	@AuthorisationStatus nvarchar(250) = NULL,
	@EVCode nvarchar(250) = NULL,
	@XevprmStatus nvarchar(250) = NULL,

	@Operation nvarchar(250) = NULL,
	@Action nvarchar(250) = NULL,
	@XevprmXml nvarchar(250) = NULL,
	@GatewaySubmissionStatus nvarchar(250) = NULL,
	@gateway_submission_date nvarchar(250) = NULL,
	@AckXml nvarchar(250) = NULL,
	@gateway_ack_date nvarchar(250) = NULL,
	@SenderID nvarchar(250) = NULL,
	@Export nvarchar(250) = NULL,
	
	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'xevprm_message_PK DESC'
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
	Xevprm.* FROM
	(
		SELECT DISTINCT
		x.xevprm_message_PK,
		x.message_number,
		x.message_status_FK,
		x.message_creation_date,
		x.user_FK,
		x.sender_ID AS SenderID,
		x.ack_type,
		x.gateway_submission_date,
		x.gateway_ack_date,
		x.submitted_FK,
		xAp.ap_FK,
		xAp.ap_name as AuthorisedProduct,
		xAp.package_description as PackageDescription,
		xAp.authorisation_country_code as Country,
		xAp.related_product_FK as product_FK,
		xAp.related_product_name as Product,
		xAp.licence_holder as LicenseHolder,
		xAp.authorisation_status as AuthorisationStatus,
		xAp.authorisation_number as AuthorisationNumber,
		xAp.operation_type,
		xAp.ev_code AS EVCode,

		CASE
			WHEN x.message_status_FK = 14 and x.ack_type = 1 then ''Successful''
			WHEN x.message_status_FK = 14 and x.ack_type = 2 then ''Errors''
			WHEN x.message_status_FK = 14 and x.ack_type = 3 then ''Failed''
			WHEN x.message_status_FK = 14 then ''Failed''
			else xs.xevprm_grid_status_name
		END AS XevprmStatus,

		CASE
			WHEN x.message_status_FK = 11 or x.message_status_FK = 12 then ''ACK0'' + CONVERT(nvarchar, x.ack_type) + '' received''
			WHEN x.message_status_FK = 13 then ''ACK0'' + CONVERT(nvarchar, x.ack_type) + '' delivery failed''
			WHEN x.message_status_FK = 14 then ''ACK0'' + CONVERT(nvarchar, x.ack_type) + '' delivered''
			else xs.xevprm_grid_gateway_status_name
		END AS GatewaySubmissionStatus,

		xop.name AS Operation,' +
		CASE 
			WHEN @Action IS NULL AND @Export IS NULL THEN ' 
		'''' AS Action,'
			ELSE '
		CASE
			WHEN x.message_status_FK = 1 OR x.message_status_FK = 2 THEN ''Validate''
			WHEN x.message_status_FK = 3 THEN ''Submit''
			WHEN x.message_status_FK = 4 OR x.message_status_FK = 8 OR x.message_status_FK = 10 THEN ''Abort''
			WHEN x.message_status_FK = 6 OR x.message_status_FK = 7 OR x.message_status_FK = 9 OR x.message_status_FK = 13 THEN ''Resubmit''
			ELSE ''-''
		END AS Action,'
		END + '

		CASE WHEN x.[xml] IS NOT NULL AND x.message_status_FK != 1 AND x.message_status_FK != 2 
			THEN ''XML PDF RTF''
			ELSE null
		END AS XevprmXml,
		CASE WHEN x.[xml] IS NOT NULL AND x.message_status_FK != 1 AND x.message_status_FK != 2 
			THEN ''PDF''
			ELSE null
		END AS XevprmXmlPdf,
		CASE WHEN x.[xml] IS NOT NULL AND x.message_status_FK != 1 AND x.message_status_FK != 2 
			THEN ''RTF''
			ELSE null
		END AS XevprmXmlRtf,

		CASE WHEN x.ack IS NOT NULL THEN
			''0'' + CONVERT(nvarchar, x.ack_type) + '' XML'' +  
			'' 0'' + CONVERT(nvarchar, x.ack_type) + '' PDF'' +  
			'' 0'' + CONVERT(nvarchar, x.ack_type) + '' RTF'' 
			ELSE null
		END AS AckXml,
		CASE WHEN x.ack IS NOT NULL THEN
			''0'' + CONVERT(nvarchar, x.ack_type) + '' PDF''
			ELSE null
		END AS AckXmlPdf,
		CASE WHEN x.ack IS NOT NULL THEN
			''0'' + CONVERT(nvarchar, x.ack_type) + '' RTF'' 
			ELSE null
		END AS AckXmlRtf,

		CASE WHEN x.submitted_FK IS NOT NULL THEN
			(	SELECT dbo.PERSON.name + '' '' + dbo.PERSON.familyname
				FROM dbo.[USER]
				LEFT JOIN dbo.PERSON ON dbo.PERSON.person_PK = dbo.[USER].person_FK
				WHERE dbo.[USER].user_PK = x.submitted_FK	)
			ELSE null
		END AS SubmittedBy,
		'''' as [Delete],
		ROW_NUMBER() OVER(PARTITION BY ap_FK ORDER BY xevprm_message_PK DESC) AS XevprmNum

	FROM dbo.XEVPRM_MESSAGE x
		LEFT JOIN dbo.XEVPRM_ENTITY_DETAILS_MN xMn ON xMn.xevprm_message_FK = x.xevprm_message_PK
		LEFT JOIN dbo.XEVPRM_AP_DETAILS xAp ON xAp.xevprm_ap_details_PK = xMn.xevprm_entity_details_FK
		LEFT JOIN dbo.XEVPRM_MESSAGE_STATUS xs ON xs.xevprm_message_status_PK = x.message_status_FK
		LEFT JOIN dbo.XEVPRM_OPERATION_TYPE xop ON xop.xevprm_operation_type_PK = xAp.operation_type
		WHERE xMn.xevprm_entity_type_FK = 1 AND x.deleted != 1'

		IF @QueryBy = 'AuthorisedProduct' 
		BEGIN 
			SET @Query = @Query +
			' AND xAp.ap_FK = ' + CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END
		END

		IF @QueryBy = 'Product' 
		BEGIN 
			SET @Query = @Query +
			' AND xAp.related_product_FK = ' + CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END
		END

		SET @Query = @Query + '
	) AS Xevprm 
	'
	SET @TempWhereQuery = '';

		-- @message_number
		IF @message_number IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.message_number LIKE N''%' + REPLACE(REPLACE(@message_number,'[','[[]'),'''','''''') + '%'''
		END

		-- @AuthorisedProduct
		IF @AuthorisedProduct IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.AuthorisedProduct LIKE N''%' + REPLACE(REPLACE(@AuthorisedProduct,'[','[[]'),'''','''''') + '%'''
		END

		-- @PackageDescription
		IF @PackageDescription IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.PackageDescription LIKE N''%' + REPLACE(REPLACE(@PackageDescription,'[','[[]'),'''','''''') + '%'''
		END

		-- @AuthorisationNumber
		IF @AuthorisationNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.AuthorisationNumber LIKE N''%' + REPLACE(REPLACE(@AuthorisationNumber,'[','[[]'),'''','''''') + '%'''
		END

		-- @Country
		IF @Country IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.Country LIKE N''%' + REPLACE(REPLACE(@Country,'[','[[]'),'''','''''') + '%'''
		END

		-- @Product
		IF @Product IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.Product LIKE N''%' + REPLACE(REPLACE(@Product,'[','[[]'),'''','''''') + '%'''
		END

		-- @LicenseHolder
		IF @LicenseHolder IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.LicenseHolder LIKE N''%' + REPLACE(REPLACE(@LicenseHolder,'[','[[]'),'''','''''') + '%'''
		END

		-- @AuthorisationStatus
		IF @AuthorisationStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.AuthorisationStatus LIKE N''%' + REPLACE(REPLACE(@AuthorisationStatus,'[','[[]'),'''','''''') + '%'''
		END

		-- @EVCode
		IF @EVCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.EVCode LIKE N''%' + REPLACE(REPLACE(@EVCode,'[','[[]'),'''','''''') + '%'''
		END

		-- @XevprmStatus
		IF @XevprmStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.XevprmStatus LIKE N''' + REPLACE(REPLACE(@XevprmStatus,'[','[[]'),'''','''''') + ''''
		END

		-- @Operation
		IF @Operation IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.Operation LIKE N''' + REPLACE(REPLACE(@Operation,'[','[[]'),'''','''''') + ''''
		END

		-- @Action
		IF @Action IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.Action LIKE N''%' + REPLACE(REPLACE(@Action,'[','[[]'),'''','''''') + '%'''
		END

		-- @XevprmXml
		IF @XevprmXml IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.XevprmXml LIKE N''%' + REPLACE(REPLACE(@XevprmXml,'[','[[]'),'''','''''') + '%'''
		END

		-- @GatewaySubmissionStatus
		IF @GatewaySubmissionStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.GatewaySubmissionStatus = ''' + REPLACE(REPLACE(@GatewaySubmissionStatus,'[','[[]'),'''','''''') + ''''
		END

		-- @gateway_submission_date
		IF @gateway_submission_date IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SUBSTRING(CONVERT(NVARCHAR(30),Xevprm.gateway_submission_date,4) + '' '' + CONVERT(NVARCHAR(30),Xevprm.gateway_submission_date,108),0,15) LIKE ''%' + REPLACE(REPLACE(@gateway_submission_date,'[','[[]'),'''','''''') + '%'''
		END	

		-- @AckXml
		IF @AckXml IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.AckXml LIKE N''%' + REPLACE(REPLACE(@AckXml,'[','[[]'),'''','''''') + '%'''
		END

		-- @gateway_ack_date
		IF @gateway_ack_date IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SUBSTRING(CONVERT(NVARCHAR(30),Xevprm.gateway_ack_date,4) + '' '' + CONVERT(NVARCHAR(30),Xevprm.gateway_ack_date,108),0,15) LIKE ''%' + REPLACE(REPLACE(@gateway_ack_date,'[','[[]'),'''','''''') + '%'''
		END	

		-- @SenderID
		IF @SenderID IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Xevprm.SenderID LIKE N''%' + REPLACE(REPLACE(@SenderID,'[','[[]'),'''','''''') + '%'''
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