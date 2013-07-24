-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ap_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[ap_PK], [product_FK], [authorisationcountrycode_FK], [organizationmahcode_FK],
		 [product_name], [productshortname], [authorisationnumber], [authorisationstatus_FK],
		  [authorisationdate], [authorisationexpdate], [comment], [authorisationwithdrawndate], 
		  [packagedesc], [marketed], [legalstatus], [withdrawndateformat], [mflcode_FK], [qppvcode_person_FK],
		   [product_ID], [ev_code], [XEVPRM_status], [responsible_user_person_FK], [launchdate], [description], 
		   [authorised_product_ID], [authorisationdateformat], [evprm_comments], [localnumber], [ap_ID], [shelflife], 
		   [productgenericname], [productcompanyname], [productstrenght], [productform], [infodate], [phv_email] , [phv_phone], 
		   [article_57_reporting], [sunsetclause],[substance_translations], [qppv_code_FK], [local_representative_FK], [Indications], [local_qppv_code_FK]
		   ,[license_holder_group_FK], [reservation_confirmed], [reserved_to], [local_codes], [pack_size], [reimbursment_status]
		FROM [dbo].[AUTHORISED_PRODUCT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AUTHORISED_PRODUCT]
END
